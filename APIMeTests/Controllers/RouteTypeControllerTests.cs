using Microsoft.VisualStudio.TestTools.UnitTesting;
using APIMe.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIMe.Entities.Models;
using APIMe.JwtFeatures;
using APIMeTests;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Security.Claims;
using APIMe.Services.Routes;
using Microsoft.AspNetCore.Mvc;
using APIMe.Entities.DataTransferObjects.Admin.Route;
using APIMe.Mapping;

namespace APIMe.Controllers.Tests
{

    [TestClass()]
    public class RouteTypeControllerTests
    {
        private RouteTypeController routeType;

        public RouteTypeControllerTests()
        {
            var mockStore = new Mock<IUserStore<IdentityUser>>();

            var userManager = new Mock<UserManager<IdentityUser>>(mockStore.Object, null, null, null, null, null, null, null, null);

            JwtHandler jwtHandler = MockJwt.GetJwtHandler();
            APIMeContext ContextDataAccess = new MockContext<APIMeContext>().GetMockContext();


            var mapper=new Mock<IMapper>();
            RouteService route = new RouteService(ContextDataAccess,mapper.Object);

            routeType = new RouteTypeController(userManager.Object, ContextDataAccess, route, mapper.Object);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testingmockup"),
            }, "mock"));
            routeType.ControllerContext.HttpContext = new DefaultHttpContext() { User = user };
        }
        [TestMethod()]
        public async Task GetRouteTypesTest()
        {
            ActionResult<IEnumerable<RouteDto>> result = await routeType.GetRouteTypes();
            Assert.IsNotNull(result);
            Assert.AreEqual(200, actual: ((OkObjectResult)result.Result).StatusCode);
        }
    }
}