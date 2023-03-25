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
    [Route("routeTypeApi")]
    public class RouteTypeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IMapper _mapper;
        private readonly RouteService _routeService;
        private APIMeContext _aPIMeContext;

        public RouteTypeController(UserManager<IdentityUser> userManager, APIMeContext aPIMeContext, RouteService routeService, IMapper mapper)
        {
            _userManager = userManager;
            _aPIMeContext = aPIMeContext;
            _routeService = routeService;
            _mapper = mapper;
        }

        [HttpGet("routeTypes")]
        public async Task<ActionResult<IEnumerable<RouteDto>>> GetRouteTypes()
        {
            var routesTypes = await _routeService.GetRouteTypesAsync();
            var routeTypesDtos = _mapper.Map<IEnumerable<RouteTypeDto>>(routesTypes);
            foreach(RouteTypeDto item in routeTypesDtos)
            {
                item.CrudActionName = CrudActions.Actions.Where(ca => ca.Id == item.CrudId).FirstOrDefault().Action;
            }

            return Ok(routeTypesDtos);
        }
    }
}
