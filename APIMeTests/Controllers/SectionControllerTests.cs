using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Identity;
using APIMe.Entities.Models;
using APIMe.JwtFeatures;
using Moq;
using APIMe.Entities.DataTransferObjects.Admin.Section;
using Microsoft.AspNetCore.Mvc;
using APIMeTests;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace APIMe.Controllers.Tests

{


    [TestClass()]
    public class SectionControllerTests
    {
        private SectionController controller;

        public SectionControllerTests()
        {
            var mockStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new UserManager<IdentityUser>(mockStore.Object, null, null, null, null, null, null, null, null);
            JwtHandler jwtHandler = MockJwt.GetJwtHandler();
            APIMeContext ContextDataAccess = new MockContext<APIMeContext>().GetMockContext();

            controller = new SectionController(userManager, ContextDataAccess, jwtHandler);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testingmockup"),
            }, "mock"));
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = user };
        }

        [TestMethod()]
            public async Task GetSectionsTest()
        {
            ActionResult<List<SectionDTO>> result = await controller.GetSections();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(List<SectionDTO>));
        }
        [TestMethod()]
            public async Task AddSectionTest()
        {
            SectionDTO section = new SectionDTO();
            section.Id = -1;
            section.SectionName = "Test";
            section.NumberOfStudents = 1;
            section.ProfessorName = "Test";
            section.AccessCode = "Test";
            ActionResult<SectionDTO> result = await controller.AddSection(section);
            Assert.IsNotNull(result);            
            Assert.AreEqual(201, actual: ((CreatedAtActionResult)result.Result).StatusCode);
        }

        [TestMethod()]
            public async Task EditSectionTest()
        {
            SectionDTO section = new SectionDTO();
            section.Id = 4;
            section.SectionName = "Test";
            section.NumberOfStudents = 1;
            section.ProfessorName = "Test";
            section.AccessCode = "Test";
            ActionResult<SectionDTO> result = await controller.EditSection(4,section);
            Assert.IsNotNull(result);
            Assert.AreEqual(200, actual: ((OkResult)result.Result).StatusCode);
        }

        [TestMethod()]
            public async Task DeleteSectionTest()
        {       
            IActionResult result = await controller.DeleteSection(4);
            Assert.IsNotNull(result);
            Assert.AreEqual(204, actual: ((NoContentResult)result).StatusCode);
        }

        [TestMethod()]
            public void PrivacyTest()
        {
            
            IActionResult result = controller.Privacy();
            Assert.IsNotNull(result);
            Assert.AreEqual(200, actual: ((OkObjectResult)result).StatusCode);
        }
    }
}