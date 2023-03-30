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
using APIMe.Entities.DataTransferObjects.Admin.Route;
using Microsoft.AspNetCore.Mvc;

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
            mapper.Setup(p => p.Map<RouteDto>(It.IsAny<Route>())).Returns(new RouteDto() { Id = 1, DataTableName = "", Description = "", IsVisible = true, Name = "test", RouteTypeId = 1 });
            mapper.Setup(p => p.Map<Route>(It.IsAny<RouteDto>())).Returns(new Route() { DataTableName = "Products", IsVisible = true, Id = 7, Description = "NewRoute", Name = "New", RouteTypeId = 1 });


            RouteService routeService = new RouteService(ContextDataAccess, mapper.Object);

            route = new RouteController(userManager.Object, ContextDataAccess, routeService, mapper.Object);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testingmockup"),
            }, "mock"));
            route.ControllerContext.HttpContext = new DefaultHttpContext() { User = user };
        }

        [TestMethod()]
        public async Task GetRoutesTest()
        {
            ActionResult<IEnumerable<RouteDto>> result = await route.GetRoutes();
            Assert.IsNotNull(result);
            Assert.AreEqual(200, actual: ((OkObjectResult)result.Result).StatusCode);
        }

        [TestMethod()]
        public async Task GetDataSourcesTest()
        {
            ActionResult<IEnumerable<RouteDto>> result = await route.GetDataSources();
            Assert.IsNotNull(result);
            Assert.AreEqual(200, actual: ((OkObjectResult)result.Result).StatusCode);
        }

        [TestMethod()]
        public async Task TestRouteTest()
        {
            Route details= new Route() { DataTableName = "Products", IsVisible = true, Id = 1, Description = "NewRoute", Name = "New", RouteTypeId = 1 };
            ActionResult<TestRouteResponse> result = await route.TestRoute(1,details);
            Assert.IsNotNull(result);
            Assert.AreEqual(200, actual: ((OkObjectResult)result.Result).StatusCode);
        }

        [TestMethod()]
        public async Task GetPropertiesByRouteIdTest()
        {
            ActionResult<IEnumerable<Property>> result = await route.GetPropertiesByRouteId(1);
            Assert.IsNotNull(result);
            Assert.AreEqual(200, actual: ((OkObjectResult)result.Result).StatusCode);
        }

        [TestMethod()]
        public async Task GetRouteTest()
        {
            ActionResult<RouteDto> result = await route.GetRoute(1);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
        }

        [TestMethod()]
        public async Task CreateRouteTest()
        {
            RouteDto dto = new RouteDto() { DataTableName="Products", IsVisible=true, Id=7, Description="NewRoute", Name="New", RouteTypeId=1};
            ActionResult<RouteDto> result = await route.CreateRoute(dto);
            Assert.IsNotNull(result);
            Assert.AreEqual(201, actual: ((CreatedAtActionResult)result.Result).StatusCode);
        }

        [TestMethod()]
        public async Task UpdateRouteTest()
        {
            RouteDto dto = new RouteDto() { DataTableName = "Products", IsVisible = true, Id = 7, Description = "NewRoute", Name = "New", RouteTypeId = 1 };
            await route.CreateRoute(dto);
            ActionResult result = (ActionResult)await route.UpdateRoute(7,dto);
            Assert.IsNotNull(result);
            Assert.AreEqual(204, actual: ((NoContentResult)result).StatusCode);
        }

        [TestMethod()]
        public async Task DeleteRouteTest()
        {
            ActionResult result = (ActionResult)await route.DeleteRoute(1);
            Assert.IsNotNull(result);
            Assert.AreEqual(204, actual: ((NoContentResult)result).StatusCode);
        }

        [TestMethod()]
        public async Task ToggleVisibilityTest()
        {
            ActionResult result = (ActionResult)await route.ToggleVisibility(1);
            Assert.IsNotNull(result);
            Assert.AreEqual(204, actual: ((NoContentResult)result).StatusCode);
        }
    }
}