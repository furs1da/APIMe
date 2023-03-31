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
using Microsoft.AspNetCore.Identity;
using Moq;
using APIMe.Entities.DataTransferObjects.Admin.Route;
using Microsoft.AspNetCore.Mvc;
using APIMe.Entities.DataTransferObjects.Admin.Section;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace APIMe.Controllers.Tests
{
    [TestClass()]
    public class ProfileControllerTests
    {
        ProfileController profileController;
        public ProfileControllerTests()
        {
            var mockStore = new Mock<IUserStore<IdentityUser>>();

            var userManager = new Mock<UserManager<IdentityUser>>(mockStore.Object, null, null, null, null, null, null, null, null);
            JwtHandler jwtHandler = MockJwt.GetJwtHandler();
            APIMeContext ContextDataAccess = new MockContext<APIMeContext>().GetMockContext();

            profileController = new ProfileController(userManager.Object, ContextDataAccess, jwtHandler);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testingmockup"),
            }, "mock"));
            profileController.ControllerContext.HttpContext = new DefaultHttpContext() { User = user };
        }
        [TestMethod()]
        public async Task GetProfessorProfileTest()
        {
            ActionResult<Professor> result= await profileController.GetProfessorProfile();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(Professor));
        }

        [TestMethod()]
        public async Task EditProfessorProfileTest()
        {
            ProfileDTO profile = new ProfileDTO() { Id=0, Email="NewEmail", FirstName="Name",LastName="Name" };
            ActionResult<ProfileDTO> result = await profileController.EditProfessorProfile(0,profile);
            Assert.IsNotNull(result);
            Assert.AreEqual(200, actual: ((OkResult)result.Result).StatusCode);
        }
    }
}