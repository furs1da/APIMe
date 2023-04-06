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
using APIMe.Entities.DataTransferObjects.Authorization;
using APIMe.Entities.DataTransferObjects.Admin.Section;
using Duende.IdentityServer.Models;
using static System.Collections.Specialized.BitVector32;
using System.Security.Claims;
using APIMe.Entities.DataTransferObjects.Admin.Profile;
using static Duende.IdentityServer.Events.TokenIssuedSuccessEvent;

namespace APIMe.Controllers
{
    [ApiController]
    [Route("profileApi")]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JwtHandler _jwtHandler;
        private APIMeContext _aPIMeContext;

        public ProfileController(UserManager<IdentityUser> userManager, APIMeContext aPIMeContext, JwtHandler jwtHandler, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
            _aPIMeContext = aPIMeContext;
            _signInManager = signInManager;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("profile")]
        public async Task<ActionResult<Professor>> GetProfessorProfile()
        {
            var professors = await _aPIMeContext.Professors.ToListAsync();

            string debug = User.Identity.Name;

            var professor = await _aPIMeContext.Professors
                .SingleOrDefaultAsync(p => p.Email == debug);

            if (professor == null)
            {
                    return BadRequest("The professor does not exist.");                
            }

            return professor;
        }










        [Authorize(Roles = "Administrator")]
        [HttpPut("edit/{id}")]
        public async Task<ActionResult> EditProfessorProfile(int id, [FromBody] ProfessorProfileDTO professorProfile)
        {
            var professor = await _aPIMeContext.Professors
                .SingleOrDefaultAsync(p => p.Id == id);

            if (professor == null)
            {
                return BadRequest("The professor does not exist.");
            }

            if (!string.IsNullOrEmpty(professorProfile.Email))
            {
                // Validate the email format
                if (IsValidEmail(professorProfile.Email))
                {
                    // Check if a user with the new email value already exists
                    var existingUser = await _userManager.FindByEmailAsync(professorProfile.Email);
                    if (existingUser != null && existingUser.UserName != professor.Email)
                    {
                        return BadRequest("The email address is already in use.");
                    }

                    // Update the email in the AspNetUsers table
                    var user = await _userManager.FindByEmailAsync(professor.Email);

                    if (user != null)
                    {
                        user.Email = professorProfile.Email;

                        // Check if the existing UserName value is the same as the old email value
                        if (user.UserName == professor.Email)
                        {
                            user.UserName = professorProfile.Email;
                            await _userManager.UpdateNormalizedUserNameAsync(user);
                        }

                        await _userManager.UpdateNormalizedEmailAsync(user);
                        await _userManager.UpdateAsync(user);

                        // Refresh the user's claims by generating a new JWT token
                        var signingCredentials = _jwtHandler.GetSigningCredentials();
                        var claims = await _jwtHandler.GetClaims(user);
                        var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
                        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                        // Update the professor's email
                        professor.Email = professorProfile.Email;

                        // Update the professor's first and last name if provided
                        if (!string.IsNullOrEmpty(professorProfile.FirstName))
                        {
                            professor.FirstName = professorProfile.FirstName;
                        }
                        if (!string.IsNullOrEmpty(professorProfile.LastName))
                        {
                            professor.LastName = professorProfile.LastName;
                        }

                        await _aPIMeContext.SaveChangesAsync();

                        // Return the new token as part of the response
                        return Ok(new { Token = token });
                    }
                    else
                    {
                        return BadRequest("The user associated with the professor does not exist.");
                    }
                }
            }

            // Update the professor's first and last name if provided
            if (!string.IsNullOrEmpty(professorProfile.FirstName))
            {
                professor.FirstName = professorProfile.FirstName;
            }
            if (!string.IsNullOrEmpty(professorProfile.LastName))
            {
                professor.LastName = professorProfile.LastName;
            }

            await _aPIMeContext.SaveChangesAsync();

            return Ok();
        }




        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }



        [Authorize(Roles = "Student")]
        [HttpGet("studentProfile")]
        public async Task<ActionResult<Student>> GetStudentProfile()
        {
            var students = await _aPIMeContext.Professors.ToListAsync();

            string debug = User.Identity.Name;

            var student = await _aPIMeContext.Students
                .SingleOrDefaultAsync(p => p.Email == debug);

            if (student == null)
            {
                return BadRequest("The student does not exist.");
            }

            return student;
        }






        [Authorize(Roles = "Student")]
        [HttpPut("editStudent/{id}")]
        public async Task<ActionResult<StudentProfileDTO>> EditStudentProfile(int id, [FromBody] StudentProfileDTO studentProfile)
        {
            var student = await _aPIMeContext.Students
                 .SingleOrDefaultAsync(p => p.Id == id);

            if (student == null)
            {
                return BadRequest("The student does not exist.");
            }

            if (!string.IsNullOrEmpty(studentProfile.Email))
            {
                // Validate the email format
                if (IsValidEmail(studentProfile.Email))
                {
                    // Check if a user with the new email value already exists
                    var existingUser = await _userManager.FindByEmailAsync(studentProfile.Email);
                    if (existingUser != null && existingUser.UserName != student.Email)
                    {
                        return BadRequest("The email address is already in use.");
                    }

                    // Update the email in the AspNetUsers table
                    var user = await _userManager.FindByEmailAsync(student.Email);

                    if (user != null)
                    {
                        user.Email = studentProfile.Email;

                        // Check if the existing UserName value is the same as the old email value
                        if (user.UserName == student.Email)
                        {
                            user.UserName = studentProfile.Email;
                            await _userManager.UpdateNormalizedUserNameAsync(user);
                        }

                        await _userManager.UpdateNormalizedEmailAsync(user);
                        await _userManager.UpdateAsync(user);

                        // Refresh the user's claims by generating a new JWT token
                        var signingCredentials = _jwtHandler.GetSigningCredentials();
                        var claims = await _jwtHandler.GetClaims(user);
                        var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
                        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                        // Update the professor's email
                        student.Email = studentProfile.Email;

                        // Update the professor's first and last name if provided
                        if (!string.IsNullOrEmpty(studentProfile.FirstName))
                        {
                            student.FirstName = studentProfile.FirstName;
                        }
                        if (!string.IsNullOrEmpty(studentProfile.LastName))
                        {
                            student.LastName = studentProfile.LastName;
                        }
                        if (studentProfile.StudentId != 0)
                        {
                            student.StudentId = studentProfile.StudentId;
                        }

                        await _aPIMeContext.SaveChangesAsync();

                        // Return the new token as part of the response
                        return Ok(new { Token = token });
                    }
                    else
                    {
                        return BadRequest("The user associated with the professor does not exist.");
                    }
                }
            }

            // Update the professor's first and last name if provided
            if (!string.IsNullOrEmpty(studentProfile.FirstName))
            {
                student.FirstName = studentProfile.FirstName;
            }
            if (!string.IsNullOrEmpty(studentProfile.LastName))
            {
                student.LastName = studentProfile.LastName;
            }
            if (studentProfile.StudentId != 0)
            {
                student.StudentId = studentProfile.StudentId;
            }


            await _aPIMeContext.SaveChangesAsync();

            return Ok();
        }
    }
}