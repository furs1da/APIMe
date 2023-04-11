using APIMe.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using APIMe.Entities.Models;
using APIMe.JwtFeatures;
using APIMeTests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Security.Claims;
using APIMe.Services.Routes;
using AutoMapper;
using APIMe.Entities.DataTransferObjects.Admin.Route;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

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


            MockRouteService routeService = new MockRouteService(ContextDataAccess, mapper.Object);
            RouteLogService logService=new RouteLogService(ContextDataAccess,mapper.Object);
            route = new RouteController(userManager.Object, ContextDataAccess, routeService, mapper.Object,logService);
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
            Route details = new Route() { DataTableName = "Products", IsVisible = true, Id = 1, Description = "NewRoute", Name = "New", RouteTypeId = 1 };
            ActionResult<TestRouteResponse> result = await route.TestRoute(1, details);
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
            Assert.IsInstanceOfType(result.Value, typeof(RouteDto));
        }

        [TestMethod()]
        public async Task CreateRouteTest()
        {
            RouteDto dto = new RouteDto() { DataTableName = "Products", IsVisible = true, Id = 7, Description = "NewRoute", Name = "New", RouteTypeId = 1 };
            ActionResult<RouteDto> result = await route.CreateRoute(dto);
            Assert.IsNotNull(result);
            Assert.AreEqual(201, actual: ((CreatedAtActionResult)result.Result).StatusCode);
        }

        [TestMethod()]
        public async Task UpdateRouteTest()
        {
            RouteDto dto = new RouteDto() { DataTableName = "Products", IsVisible = true, Id = 7, Description = "NewRoute", Name = "New", RouteTypeId = 1 };
            await route.CreateRoute(dto);
            ActionResult result = (ActionResult)await route.UpdateRoute(7, dto);
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
        public async Task GetRecordsFromTableTest()
        {
            ActionResult<IEnumerable<Object>> result = await route.GetRecordsFromTable("Products");
            Assert.IsNotNull(result);
            Assert.AreEqual(200, actual: ((OkObjectResult)result.Result).StatusCode);
            result = await route.GetRecordsFromTable("Customers",1);
            Assert.IsNotNull(result);
            Assert.AreEqual(200, actual: ((OkObjectResult)result.Result).StatusCode);
        }

        [TestMethod()]
        public async Task GetRecordByIdFromTableTest()
        {
            ActionResult<Object> result = await route.GetRecordByIdFromTable("Products",1);
            Assert.IsNotNull(result);
            Assert.AreEqual(200, actual: ((OkObjectResult)result.Result).StatusCode);
        }

        [TestMethod()]
        public async Task AddRecordToTableTest()
        {
            Products products=new Products { Description="New", Id=-1, Name="New", Price=128.42m, Quantity=4001 };
            var jsonElement = System.Text.Json.JsonSerializer.SerializeToElement(products);
            ActionResult<Object> result = await route.AddRecordToTable("Products",jsonElement);
            Assert.IsNotNull(result);
            Assert.AreEqual(201, actual: ((CreatedAtActionResult)result.Result).StatusCode);
        }

        [TestMethod()]
        public async Task UpdateRecordInTableTest()
        {
            Products products = new Products { Description = "New", Id = 1, Name = "New", Price = 128.42m, Quantity = 4001 };
            var jsonElement = System.Text.Json.JsonSerializer.SerializeToElement(products);
            ActionResult<Object> result = await route.UpdateRecordInTable("Products", jsonElement);
            Assert.IsNotNull(result);
            Assert.AreEqual(200, actual: ((ObjectResult)result.Result).StatusCode);
        }

        [TestMethod()]
        public async Task PatchRecordInTableTest()
        {
            Products products = new Products { Description = "New", Id = 1, Name = "New", Price = 128.42m, Quantity = 4001 };
            var jsonElement = System.Text.Json.JsonSerializer.SerializeToElement(products);
            ActionResult<Object> result = await route.PatchRecordInTable("Products",1, jsonElement);
            Assert.IsNotNull(result);
            Assert.AreEqual(200, actual: ((ObjectResult)result.Result).StatusCode);
        }

        [TestMethod()]
        public async Task DeleteRecordFromTableTest()
        {
            ActionResult<Object> result = await route.DeleteRecordFromTable("Products", 1);
            Assert.IsNotNull(result);
            Assert.AreEqual(204, actual: ((NoContentResult)result.Result).StatusCode);
        }
    }
}