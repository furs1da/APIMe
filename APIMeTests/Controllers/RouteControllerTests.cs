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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Security.Claims;
using APIMe.Services.Routes;
using AutoMapper;
using APIMe.Migrations;

namespace APIMe.Controllers.Tests
{
    [TestClass()]
    public class RouteControllerTests
    {

        private RouteController route;

        public RouteControllerTests()
        {
            var mockStore = new Mock<IUserStore<IdentityUser>>();

            var userManager = new Mock<UserManager<IdentityUser>>(mockStore.Object, null, null, null, null, null, null, null, null);

            JwtHandler jwtHandler = MockJwt.GetJwtHandler();
            APIMeContext ContextDataAccess = new MockContext<APIMeContext>().GetMockContext();


            var mapper = new Mock<IMapper>();
            RouteService routeService = new RouteService(ContextDataAccess, mapper.Object);

            route = new RouteController(userManager.Object, ContextDataAccess, routeService, mapper.Object);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testingmockup"),
            }, "mock"));
            route.ControllerContext.HttpContext = new DefaultHttpContext() { User = user };
        }

        [TestMethod()]
        public void GetRoutesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetDataSourcesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void TestRouteTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetPropertiesByRouteIdTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetRouteTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateRouteTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateRouteTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteRouteTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ToggleVisibilityTest()
        {
            Assert.Fail();
        }
    }
}