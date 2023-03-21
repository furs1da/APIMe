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
