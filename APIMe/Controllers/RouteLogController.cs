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
using APIMe.Entities.DataTransferObjects.Admin.RouteLog;
using APIMe.Services.Routes;

namespace APIMe.Controllers
{

    [ApiController]
    [Route("routeLogApi")]
    public class RouteLogController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly JwtHandler _jwtHandler;
        private APIMeContext _aPIMeContext;
        private readonly IMapper _mapper;
        private readonly RouteLogService _routeLogService;
        public RouteLogController(UserManager<IdentityUser> userManager, APIMeContext aPIMeContext, JwtHandler jwtHandler, IMapper mapper, RouteLogService routeLogService)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
            _aPIMeContext = aPIMeContext;
            _mapper = mapper;
            _routeLogService = routeLogService;
        }

        [HttpGet("logs")]
        public async Task<ActionResult<IEnumerable<RouteLogDto>>> GetAllRouteLogs()
        {
            var routeLogs = await _routeLogService.GetAllRouteLogsAsync();
            var routeLogDtos = _mapper.Map<IEnumerable<RouteLogDto>>(routeLogs);

            return Ok(routeLogDtos);
        }

        [HttpGet("logs/byuser/{userId}")]
        public async Task<ActionResult<IEnumerable<RouteLogDto>>> GetRouteLogsByUserId(int userId)
        {
            var routeLogs = await _routeLogService.GetRouteLogsByUserIdAsync(userId);
            var routeLogDtos = _mapper.Map<IEnumerable<RouteLogDto>>(routeLogs);

            return Ok(routeLogDtos);
        }
    }
}
