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
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace APIMe.Controllers.Tests
{
    [TestClass()]
    public class AccountControllerTests
    {
        private AccountController controller;

        public AccountControllerTests()
        {

            var mockStore = new Mock<IUserStore<IdentityUser>>();

            var userManager = new Mock<UserManager<IdentityUser>>(mockStore.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(p => p.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new IdentityUser { Email = "test", EmailConfirmed = true, Id = "4", UserName = "test" });
            userManager.Setup(p => p.IsEmailConfirmedAsync(It.IsAny<IdentityUser>())).ReturnsAsync(true);
            userManager.Setup(p => p.CheckPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(true);
            userManager.Setup(p => p.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new IdentityUser { Email = "test", EmailConfirmed = true, Id = "4", UserName = "test" });
            userManager.Setup(p => p.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            userManager.Setup(p => p.ConfirmEmailAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            userManager.Setup(p => p.ResetPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success); 
           
            JwtHandler jwtHandler = MockJwt.GetJwtHandler();
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
            UserForAuthenticationDto user = new UserForAuthenticationDto
            {
                Email = "test",
                Password = "test"
            };
            IActionResult result = await controller.Login(user);
            Assert.IsNotNull(result);
            Assert.AreEqual(200, actual: ((OkObjectResult)result).StatusCode);
        }

        [TestMethod()]
        public async Task ForgotPasswordTest()
        {
            ForgotPasswordDto user = new ForgotPasswordDto
            {
                Email = "test",
                ClientURI="egg"
            };
            IActionResult result = await controller.ForgotPassword(user);
            Assert.IsNotNull(result);
            Assert.AreEqual(200, actual: ((OkResult)result).StatusCode);
        }

        [TestMethod()]
        public async Task RegisterUserTest()
        {
            Section section = new Section() { Id = 4, SectionName = "n/a", ProfessorId = 0, AccessCode = "n/a" };
            StudentSection studentSection= new StudentSection() { SectionId=4,Section=section};
            UserForRegistrationDto user = new UserForRegistrationDto
            {
                Email = "new",
                Password= "new",
                ConfirmPassword= "new",
                FirstName= "new",
                LastName= "new",
                StudentNumber="1234567",
                AccessCode="n/a",
                StudentSection=4,
                ClientURI="quaint"
            };
            IActionResult result = await controller.RegisterUser(user);
            Assert.IsNotNull(result);
            Assert.AreEqual(201, actual: ((StatusCodeResult)result).StatusCode);
        }

        [TestMethod()]
        public async Task EmailConfirmationTest()
        {
            IActionResult result = await controller.EmailConfirmation("test","token");
            Assert.IsNotNull(result);
            Assert.AreEqual(200, actual: ((OkResult)result).StatusCode);
        }

        [TestMethod()]
        public async Task ResetPasswordTest()
        {
            ResetPasswordDto reset = new ResetPasswordDto()
            {
                Email= "test",
                Password= "test",
                ConfirmPassword= "test",
                Token= "test"
            };
            IActionResult result = await controller.ResetPassword(reset);
            Assert.IsNotNull(result);
            Assert.AreEqual(200, actual: ((OkResult)result).StatusCode);
        }
    }
}