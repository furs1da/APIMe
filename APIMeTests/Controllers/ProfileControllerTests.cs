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
using APIMe.Entities.DataTransferObjects.Admin.Profile;

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
            profileController = new ProfileController(userManager.Object, ContextDataAccess, jwtHandler,null);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testingmockup"),
            }, "mock"));
            profileController.ControllerContext.HttpContext = new DefaultHttpContext() { User = user };
        }
        [TestMethod()]
        public async Task GetProfessorProfileTest()
        {
            ActionResult<Professor> result = await profileController.GetProfessorProfile();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(Professor));
        }

        [TestMethod()]
        public async Task EditProfessorProfileTest()
        {
            ProfessorProfileDTO profile = new ProfessorProfileDTO() { Id = 1, Email = "NewEmail", FirstName = "Name", LastName = "Name" };
            ActionResult<ProfessorProfileDTO> result = await profileController.EditProfessorProfile(1, profile);
            Assert.IsNotNull(result);
            Assert.AreEqual(200, actual: ((OkResult)result.Result).StatusCode);
        }

        [TestMethod()]
        public async Task GetStudentProfileTest()
        {
            ActionResult<Student> result = await profileController.GetStudentProfile();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(Student));
        }

        [TestMethod()]
        public async Task EditStudentProfileTest()
        {
            StudentProfileDTO profile = new StudentProfileDTO() { Id = 0, Email = "NewEmail", FirstName = "Name", LastName = "Name" };
            ActionResult<StudentProfileDTO> result = await profileController.EditStudentProfile(1, profile);
            Assert.IsNotNull(result);
            Assert.AreEqual(200, actual: ((OkResult)result.Result).StatusCode);
        }
    }
}