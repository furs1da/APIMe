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
/*            userManager.Setup(p => p.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new IdentityUser { Email = "test", EmailConfirmed = true, Id = "4", UserName = "test" });
            userManager.Setup(p => p.IsEmailConfirmedAsync(It.IsAny<IdentityUser>())).ReturnsAsync(true);
            userManager.Setup(p => p.CheckPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(true);
            userManager.Setup(p => p.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new IdentityUser { Email = "test", EmailConfirmed = true, Id = "4", UserName = "test" });
            userManager.Setup(p => p.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            userManager.Setup(p => p.ConfirmEmailAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            userManager.Setup(p => p.ResetPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);*/

            JwtHandler jwtHandler = MockJwt.GetJwtHandler();
            APIMeContext ContextDataAccess = new MockContext<APIMeContext>().GetMockContext();

            var route = new Mock<RouteService>();
            var mapper=new Mock<Mapper>();

            routeType = new RouteTypeController(userManager.Object, ContextDataAccess, route.Object, mapper.Object);
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
            //Assert.AreEqual(200, actual: ((ActionResult)result).StatusCode);
        }
    }
}