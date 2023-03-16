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

        public AccountController(UserManager<IdentityUser> userManager, APIMeContext aPIMeContext, JwtHandler jwtHandler)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
            _aPIMeContext = aPIMeContext;
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

                //Student emailStudentCheck = await _aPIMeContext.Students.FirstOrDefaultAsync(st => st.Email == userForRegistration.Email);

                //if (emailStudentCheck != null)
                //{
                //    IEnumerable<string>? errors = new List<string> { "Email is already in use." };
                //    return BadRequest(new RegistrationResponseDto { Errors = errors });
                //}

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
    }
}