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
using APIMe.Interfaces;
using Microsoft.AspNetCore.Mvc;
using APIMe.Services.Email;
using APIMe.Entities.DataTransferObjects.Authorization;
using System.IdentityModel.Tokens.Jwt;
using NSubstitute;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Castle.Core.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace APIMe.Controllers.Tests
{
    [TestClass()]
    public class AccountControllerTests
    {
        private AccountController controller;

        public AccountControllerTests()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            IConfiguration configuration = configurationBuilder.Build();

            var mockStore = new Mock<IUserStore<IdentityUser>>();

            var userManager = new Mock<UserManager<IdentityUser>>(mockStore.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(p => p.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new IdentityUser { Email="test", EmailConfirmed =true, Id="4", UserName ="test" });
            userManager.Setup(p => p.IsEmailConfirmedAsync(It.IsAny<IdentityUser>())).ReturnsAsync(true);
            userManager.Setup(p => p.CheckPasswordAsync(It.IsAny<IdentityUser>(),It.IsAny<string>())).ReturnsAsync(true);

            JwtHandler jwtHandler = new JwtHandler(configuration,userManager.Object);
            APIMeContext ContextDataAccess = new MockContext<APIMeContext>().GetMockContext();

            var email = new Mock<IEmailSender>();
            controller = new AccountController(userManager.Object, ContextDataAccess, jwtHandler, email.Object);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testingmockup"),
            }, "mock"));
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = user };
        }
        [TestMethod()]
        public async Task SectionListTest()
        {
            IActionResult result = await controller.SectionList();
            Assert.IsNotNull(result);
            Assert.AreEqual(200, actual: ((OkObjectResult)result).StatusCode);
        }

        [TestMethod()]
        public async Task LoginTest()
        {
            UserForAuthenticationDto user=new UserForAuthenticationDto();
            user.Email = "test";
            user.Password = "test";
            IActionResult result = await controller.Login(user);
            Assert.IsNotNull(result);
            Assert.AreEqual(200, actual: ((OkObjectResult)result).StatusCode);
        }

        [TestMethod()]
        public void ForgotPasswordTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RegisterUserTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EmailConfirmationTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ResetPasswordTest()
        {
            Assert.Fail();
        }
    }
}