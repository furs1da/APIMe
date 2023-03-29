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
            var userManager = new UserManager<IdentityUser>(mockStore.Object, null, null, null, null, null, null, null, null);
            JwtHandler jwtHandler = new JwtHandler(configuration, userManager);
            APIMeContext ContextDataAccess = new MockContext<APIMeContext>().GetMockContext();

            /*controller = new AccountController(userManager, ContextDataAccess, jwtHandler);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testingmockup"),
            }, "mock"));
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = user };*/
        }
        [TestMethod()]
        public void SectionListTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LoginTest()
        {
            Assert.Fail();
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