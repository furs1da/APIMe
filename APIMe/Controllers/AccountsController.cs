using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using APIMe.Data;
using Microsoft.Extensions.Options;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Authorization;
using APIMe.Models;
using APIMe.Interfaces;
using AutoMapper;
using APIMe.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using static Duende.IdentityServer.Models.IdentityResources;

namespace APIMe.Controllers
{

    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        private APIMeContext _aPIMeContext;
        public AccountController(UserManager<IdentityUser> userManager, APIMeContext aPIMeContext)
        {
            _userManager = userManager;

            _aPIMeContext = aPIMeContext;
        }

        [HttpGet("sectionlist")]
        public async Task<IActionResult> RegisterUser()
        {
            InformationForRegistration informationForRegistration = new InformationForRegistration();
            informationForRegistration.SectionList = await _aPIMeContext.Sections.ToListAsync();

            return Ok(informationForRegistration);
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            try
            {

                if (userForRegistration == null || !ModelState.IsValid)
                    return BadRequest();

                Section section = _aPIMeContext.Sections.FirstOrDefault(sec => sec.Id == userForRegistration.StudentSection);

                if (section == null || section.AccessCode != userForRegistration.AccessCode)
                {
                    IEnumerable<string>? errors = new List<string> { "Invalid Access Code" };
                    return BadRequest(new RegistrationResponseDto { Errors = errors });
                }

                Student emailStudentCheck = _aPIMeContext.Students.FirstOrDefault(st => st.Email == userForRegistration.Email);

                if (emailStudentCheck != null)
                {
                    IEnumerable<string>? errors = new List<string> { "Email is already in use." };
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
                    Email = userForRegistration.Email != null ? userForRegistration.Email : ""
                };

                _aPIMeContext.Students.Add(student);
                _aPIMeContext.SaveChanges();

                student = _aPIMeContext.Students.FirstOrDefault(st => st.Email == student.Email);

                if (student == null)
                {
                    IEnumerable<string>? errors = new List<string> { "User was not created." };
                    return BadRequest(new RegistrationResponseDto { Errors = errors });
                }

                StudentSection studentSection = new StudentSection
                {
                    StudentId = student.Id,
                    SectionId = (int)userForRegistration.StudentSection
                };

                _aPIMeContext.StudentSections.Add(studentSection);
                _aPIMeContext.SaveChangesAsync();
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