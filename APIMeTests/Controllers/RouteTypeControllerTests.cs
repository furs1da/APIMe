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

namespace APIMe.Controllers.Tests
{

    [TestClass()]
    public class RouteTypeControllerTests
    {
        private RouteTypeController routeType;

        public RouteTypeControllerTests()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            IConfiguration configuration = configurationBuilder.Build();
            var mockStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new UserManager<IdentityUser>(mockStore.Object, null, null, null, null, null, null, null, null);
            JwtHandler jwtHandler = new JwtHandler(configuration, userManager);
            APIMeContext ContextDataAccess = new MockContext<APIMeContext>().GetMockContext();
            SectionController controller = new SectionController(userManager, ContextDataAccess, jwtHandler);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testingmockup"),
            }, "mock"));
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = user };

            IMapper mapper;
/*            RouteService routeService = new RouteService(ContextDataAccess, mapper);
            routeType = new RouteTypeController(userManager, ContextDataAccess, routeService, mapper);*/
        }
        [TestMethod()]
        public void GetRouteTypesTest()
        {
            Assert.Fail();
        }
    }
}