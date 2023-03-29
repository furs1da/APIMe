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
using APIMe.Entities.DataTransferObjects.Admin.Route;
using APIMe.Services.Routes;
using APIMe.Entities.Constants;

namespace APIMe.Controllers
{
    [ApiController]
    [Route("routeApi")]
    public class RouteController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IMapper _mapper;
        private readonly RouteService _routeService;
        private APIMeContext _aPIMeContext;

        public RouteController(UserManager<IdentityUser> userManager, APIMeContext aPIMeContext, RouteService routeService, IMapper mapper)
        {
            _userManager = userManager;
            _aPIMeContext = aPIMeContext;
            _routeService = routeService;
            _mapper = mapper;
        }

        [HttpGet("routes")]
        public async Task<ActionResult<IEnumerable<RouteDto>>> GetRoutes()
        {      
                var routes = await _routeService.GetRoutesAsync();
                var routeDtos = _mapper.Map<IEnumerable<RouteDto>>(routes);
                return Ok(routeDtos);
        }


        [HttpGet("dataSources")]
        public async Task<ActionResult<IEnumerable<RouteDto>>> GetDataSources()
        {
            var sources = await _routeService.GetDataSourcesAsync();
         
            return Ok(sources);
        }


        [HttpPost("testRoute/{routeId}")]
        public async Task<ActionResult<TestRouteResponse>> TestRoute(int routeId, [FromBody] object values)
        {
            try
            {
                var response = await _routeService.TestRouteAsync(routeId, values);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("properties/{routeId}")]
        public async Task<ActionResult<IEnumerable<Property>>> GetPropertiesByRouteId(int routeId)
        {
            var properties = await _routeService.GetPropertiesByRouteIdAsync(routeId);
            return Ok(properties);
        }




        [HttpGet("{id}")]
        public async Task<ActionResult<RouteDto>> GetRoute(int id)
        {
            var route = await _aPIMeContext.Routes.Include(r => r.RouteType).SingleOrDefaultAsync(r => r.Id == id);

            if (route == null)
            {
                return NotFound();
            }

            var routeDto = _mapper.Map<RouteDto>(route);
            routeDto.RouteTypeName = route.RouteType.Name;
            routeDto.RouteTypeResponseCode = route.RouteType.ResponseCode;
            routeDto.RouteTypeCrudActionName = CrudActions.Actions.FirstOrDefault(r => r.Id == route.RouteType.CrudId) == null ? "" : CrudActions.Actions.FirstOrDefault(r => r.Id == route.RouteType.CrudId).Action;
            routeDto.RouteTypeCrudActionId = route.RouteType.CrudId;

            routeDto.Records = await _routeService.GetRecordsFromDataTableAsync(route.DataTableName, 10);

            return routeDto;
        }

        [HttpPost("add")]
        public async Task<ActionResult<RouteDto>> CreateRoute(RouteDto routeDto)
        {
            var route = _mapper.Map<Entities.Models.Route>(routeDto);
            await _routeService.CreateRouteAsync(route);
            var createdRouteDto = _mapper.Map<RouteDto>(route);
            return CreatedAtAction(nameof(GetRoute), new { id = createdRouteDto.Id }, createdRouteDto);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateRoute(int id, RouteDto routeDto)
        {
            if (id != routeDto.Id)
            {
                return BadRequest();
            }

            var route = _mapper.Map<Entities.Models.Route>(routeDto);

            try
            {
                await _routeService.UpdateRouteAsync(route);
            }
            catch (DbUpdateConcurrencyException) when (!_routeService.RouteExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            var result = await _routeService.DeleteRouteAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch("{id}/toggle-visibility")]
        public async Task<IActionResult> ToggleVisibility(int id)
        {
            var result = await _routeService.ToggleVisibilityAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
