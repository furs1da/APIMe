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

namespace APIMe.Controllers
{
    [ApiController]
    [Route("sectionApi")]
    public class SectionController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly JwtHandler _jwtHandler;
        private APIMeContext _aPIMeContext;

        public SectionController(UserManager<IdentityUser> userManager, APIMeContext aPIMeContext, JwtHandler jwtHandler)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
            _aPIMeContext = aPIMeContext;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("sections")]
        public async Task<ActionResult<List<SectionDTO>>> GetSections()
        {
            var sections = await _aPIMeContext.Sections
                .Select(s => new SectionDTO
                {
                    Id = s.Id,
                    SectionName = s.SectionName,
                    ProfessorName = s.Professor.FirstName + " " + s.Professor.LastName,
                    AccessCode = s.AccessCode,
                    NumberOfStudents = s.StudentSections.Count
                })
                .ToListAsync();

            return sections;
        }


        [Authorize(Roles = "Administrator")]
        [HttpPost("add")]
        public async Task<ActionResult<SectionDTO>> AddSection([FromBody] SectionDTO section)
        {
            var professor = await _aPIMeContext.Professors
                .SingleOrDefaultAsync(p => p.Email == User.Identity.Name);

            if (professor == null)
            {
                return BadRequest( "The professor does not exist." );
            }

            if (string.IsNullOrWhiteSpace(section.SectionName) || string.IsNullOrWhiteSpace(section.AccessCode))
            {
                return BadRequest("The section name and access code must not be empty." );
            }

            var existingSection = await _aPIMeContext.Sections
                .SingleOrDefaultAsync(s => s.SectionName == section.SectionName);

            if (existingSection != null)
            {
                return BadRequest("The section name must be unique." );
            }

            var newSection = new Section
            {
                SectionName = section.SectionName,
                ProfessorId = professor.Id,
                AccessCode = section.AccessCode
            };

            _aPIMeContext.Sections.Add(newSection);
            await _aPIMeContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSections), new { id = newSection.Id }, section);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("edit/{id}")]
        public async Task<ActionResult<SectionDTO>> EditSection(int id, [FromBody] SectionDTO section)
        {
            var existingSection = await _aPIMeContext.Sections
                .SingleOrDefaultAsync(s => s.Id == id);

            if (existingSection == null)
            {
                return NotFound();
            }

            var professor = await _aPIMeContext.Professors
                .SingleOrDefaultAsync(p => p.Email == User.Identity.Name);

            if (professor == null)
            {
                return BadRequest(new SectionResponseDTO { ErrorMessage = "The professor does not exist." });
            }

            if (string.IsNullOrWhiteSpace(section.SectionName) || string.IsNullOrWhiteSpace(section.AccessCode))
            {
                return BadRequest(new SectionResponseDTO { ErrorMessage = "The section name and access code must not be empty." });
            }

            var otherSection = await _aPIMeContext.Sections
                .SingleOrDefaultAsync(s => s.SectionName == section.SectionName && s.Id != id);

            if (otherSection != null)
            {
                return BadRequest(new SectionResponseDTO { ErrorMessage = "The section name must be unique." });
            }

            existingSection.SectionName = section.SectionName;
            existingSection.AccessCode = section.AccessCode;

            await _aPIMeContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSection(int id)
        {
            var section = _aPIMeContext.Sections.FirstOrDefault(s => s.Id == id);
            if (section == null)
                return NotFound();

            var studentSections = _aPIMeContext.StudentSections.Where(ss => ss.SectionId == id).Include(i => i.Student);
            _aPIMeContext.StudentSections.RemoveRange(studentSections);

            _aPIMeContext.Sections.Remove(section);
            await _aPIMeContext.SaveChangesAsync();

            if (section.Students != null)
            {
                // Delete the corresponding users
                foreach (var student in section.Students)
                {
                    var user = await _userManager.FindByEmailAsync(student.Email);
                    if (user != null)
                    {
                        await _userManager.DeleteAsync(user);
                    }
                }
            }

            return NoContent();
        }




        [HttpGet("privacy")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Privacy()
        {
            var claims = User.Claims
                .Select(c => new { c.Type, c.Value })
                .ToList();
            return Ok(claims);
        }
    }
}
