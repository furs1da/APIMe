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

namespace APIMe.Controllers
{
    [ApiController]
    [Route("profileApi")]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly JwtHandler _jwtHandler;
        private APIMeContext _aPIMeContext;

        public ProfileController(UserManager<IdentityUser> userManager, APIMeContext aPIMeContext, JwtHandler jwtHandler)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
            _aPIMeContext = aPIMeContext;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("profile")]
        public async Task<ActionResult<Professor>> GetProfessorProfile()
        {
            var professors = await _aPIMeContext.Professors.ToListAsync();

            var professor = await _aPIMeContext.Professors
                .SingleOrDefaultAsync(p => p.Email == User.Identity.Name);

            if (professor == null)
            {
                return BadRequest("The professor does not exist.");
            }

            return professor;
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("edit/{id}")]
        public async Task<ActionResult<ProfileDTO>> EditProfessorProfile(int id, [FromBody] ProfileDTO professorProfile)
        {
            var professor = await _aPIMeContext.Professors
                .SingleOrDefaultAsync(p => p.Email == User.Identity.Name);

            if (professor == null)
            {
                return BadRequest("The professor does not exist.");
            }

            if (!string.IsNullOrEmpty(professorProfile.Email))
            {
                professor.Email = professorProfile.Email;
            }
            if (!string.IsNullOrEmpty(professorProfile.FirstName))
            {
                professor.FirstName = professorProfile.FirstName;
            }
            if (!string.IsNullOrEmpty(professorProfile.LastName))
            {
                professor.LastName = professorProfile.LastName;
            }

            var identity = new ClaimsIdentity(User.Identity);
            identity.RemoveClaim(identity.FindFirst(ClaimTypes.Name));
            identity.AddClaim(new Claim(ClaimTypes.Name, professorProfile.Email));

            var newPrincipal = new ClaimsPrincipal(identity);
            HttpContext.User = newPrincipal;


            // Refresh the current user identity with the updated name
            HttpContext.User = new System.Security.Principal.GenericPrincipal(identity, new string[] { });


            await _aPIMeContext.SaveChangesAsync();

            return Ok();
        }
    }
}