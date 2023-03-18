using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Authorization;
using APIMe.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using static Duende.IdentityServer.Models.IdentityResources;
using APIMe.JwtFeatures;
using System.IdentityModel.Tokens.Jwt;
using APIMe.Entities.Models;
using APIMe.Entities.DataTransferObjects;
using APIMe.Services.Email;
using Microsoft.AspNetCore.WebUtilities;

namespace APIMe.Controllers
{

    [ApiController]
    [AllowAnonymous]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly JwtHandler _jwtHandler;
        private APIMeContext _aPIMeContext;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<IdentityUser> userManager, APIMeContext aPIMeContext, JwtHandler jwtHandler, IEmailSender emailSender)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
            _aPIMeContext = aPIMeContext;
            _emailSender = emailSender;
        }

        [HttpGet("sectionlist")]
        public async Task<IActionResult> SectionList()
        {
            try
            {
                InformationForRegistration informationForRegistration = new InformationForRegistration();
                informationForRegistration.SectionList = await _aPIMeContext.Sections.ToListAsync();

                return Ok(informationForRegistration);
            }
            catch (Exception ex)
            {
                IEnumerable<string>? errors = new List<string> { ex.Message };
                return BadRequest(new RegistrationResponseDto { Errors = errors });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userForAuthentication)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userForAuthentication.Email);
                if (user == null)
                    return BadRequest("Invalid Request");

                if (!await _userManager.IsEmailConfirmedAsync(user))
                    return Unauthorized(new AuthResponseDto { ErrorMessage = "Email is not confirmed" });

                if (user == null || !await _userManager.CheckPasswordAsync(user, userForAuthentication.Password))
                    return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid Authentication" });

                var signingCredentials = _jwtHandler.GetSigningCredentials();
                var claims = await _jwtHandler.GetClaims(user);
                var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token });
            }
            catch (Exception ex)
            {
                IEnumerable<string>? errors = new List<string> { ex.Message };
                return BadRequest(new RegistrationResponseDto { Errors = errors });
            }
            
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
                return BadRequest("Invalid Request");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string?>
                {
                    {"token", token },
                    {"email", forgotPasswordDto.Email }
                };
            var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientURI, param);
            var message = new Message(new string[] { user.Email }, "Reset password token", callback, null);
            await _emailSender.SendEmailAsync(message);

            return Ok();
        }

        [HttpPost("registration")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            try
            {

                if (userForRegistration == null || !ModelState.IsValid)
                    return BadRequest();

                Section section = await _aPIMeContext.Sections.FirstOrDefaultAsync(sec => sec.Id == userForRegistration.StudentSection);

                if (section == null || section.AccessCode != userForRegistration.AccessCode)
                {
                    IEnumerable<string>? errors = new List<string> { "Invalid Access Code" };
                    return BadRequest(new RegistrationResponseDto { Errors = errors });
                }

                if (!int.TryParse(userForRegistration.StudentNumber, out int stn))
                {
                    IEnumerable<string>? errors = new List<string> { "Student Number should be 7 digits." };
                    return BadRequest(new RegistrationResponseDto { Errors = errors });
                }

                var user = new IdentityUser { Email = userForRegistration.Email, UserName = userForRegistration.Email };
                var result = await _userManager.CreateAsync(user, userForRegistration.Password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);

                    return BadRequest(new RegistrationResponseDto { Errors = errors });
                }

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var param = new Dictionary<string, string?>
                {
                    {"token", token },
                    {"email", user.Email }
                };
                var callback = QueryHelpers.AddQueryString(userForRegistration.ClientURI, param);
                var message = new Message(new string[] { user.Email }, "Email Confirmation token", callback, null);
                await _emailSender.SendEmailAsync(message);

                int studentNumber = 0;

                Student student = new Student
                {
                    FirstName = userForRegistration.FirstName != null ? userForRegistration.FirstName : "",
                    LastName = userForRegistration.LastName != null ? userForRegistration.LastName : "",
                    StudentId = int.TryParse(userForRegistration.StudentNumber, out studentNumber) ? studentNumber : 0,
                    Email = userForRegistration.Email != null ? userForRegistration.Email : "",
                    ApiKey = "."
                };

                await _aPIMeContext.Students.AddAsync(student);
                await _aPIMeContext.SaveChangesAsync();

                student = await _aPIMeContext.Students.FirstOrDefaultAsync(st => st.Email == student.Email);

                if (student == null)
                {
                    IEnumerable<string>? errors = new List<string> { "User was not created." };
                    return BadRequest(new RegistrationResponseDto { Errors = errors });
                }

                StudentSection studentSection = new StudentSection();
                studentSection.StudentId = student.Id;
                studentSection.SectionId = (int)userForRegistration.StudentSection;


                await _aPIMeContext.StudentSections.AddAsync(studentSection);

                await _userManager.AddToRoleAsync(user, "Student");

                await _aPIMeContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                IEnumerable<string>? errors = new List<string> { ex.Message };
                return BadRequest(new RegistrationResponseDto { Errors = errors });
            }

            return StatusCode(201);
        }


        [HttpGet("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("Invalid Email Confirmation Request");
            var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!confirmResult.Succeeded)
                return BadRequest("Invalid Email Confirmation Request");
            return Ok();
        }


        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return BadRequest("Invalid Request");
            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
            if (!resetPassResult.Succeeded)
            {
                var errors = resetPassResult.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }
            return Ok();
        }
    }
}