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

        //[HttpPost("Registration")]
        //public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        //{
        //    if (userForRegistration == null || !ModelState.IsValid)
        //        return BadRequest();

        //    var user = _mapper.Map<IdentityUser>(userForRegistration);
        //    var result = await _userManager.CreateAsync(user, userForRegistration.Password);
        //    if (!result.Succeeded)
        //    {
        //        var errors = result.Errors.Select(e => e.Description);

        //        return BadRequest(new RegistrationResponseDto { Errors = errors });
        //    }

        //    return StatusCode(201);
        //}
    }
}