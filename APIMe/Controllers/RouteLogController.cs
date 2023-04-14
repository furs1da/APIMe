using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using APIMe.JwtFeatures;
using APIMe.Entities.Models;
using APIMe.Entities.DataTransferObjects.Admin.RouteLog;
using APIMe.Services.Routes;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace APIMe.Controllers
{
    [ApiController]
    [Authorize(Roles = "Administrator")]
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

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearAllRouteLogs()
        {
            var routeLogs = await _aPIMeContext.RouteLogs.ToListAsync();
            _aPIMeContext.RouteLogs.RemoveRange(routeLogs);
            await _aPIMeContext.SaveChangesAsync();

            return Ok();
        }

    }
}
