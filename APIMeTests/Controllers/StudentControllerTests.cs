using Microsoft.VisualStudio.TestTools.UnitTesting;
using APIMe.Entities.Models;
using APIMe.JwtFeatures;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using APIMeTests;
using Moq;
using Microsoft.AspNetCore.Mvc;
using APIMe.Entities.DataTransferObjects.Admin.Student;
using APIMe.Entities.DataTransferObjects.Admin.Section;

namespace APIMe.Controllers.Tests
{
    [TestClass()]
    public class StudentControllerTests
    {
        StudentController studentController;
        public StudentControllerTests() {
            var mockStore = new Mock<IUserStore<IdentityUser>>();

            var userManager = new Mock<UserManager<IdentityUser>>(mockStore.Object, null, null, null, null, null, null, null, null);
            JwtHandler jwtHandler = MockJwt.GetJwtHandler();
            APIMeContext ContextDataAccess = new MockContext<APIMeContext>().GetMockContext();
            var mapper = new Mock<IMapper>();
            mapper.Setup(p => p.Map<Student>(It.IsAny<StudentDto>())).Returns(new Student() { Id = 7, FirstName = "test", LastName = "test", StudentId = 1234567, Email = "test", ApiKey = "t" });

            studentController = new StudentController(userManager.Object,ContextDataAccess,jwtHandler,mapper.Object);
        }

        [TestMethod()]
        public async Task GetStudentsTest()
        {
            ActionResult<IEnumerable<StudentDto>> result = await studentController.GetStudents();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(IEnumerable<StudentDto>));
        }

        [TestMethod()]
        public async Task GetSectionsTest()
        {
            ActionResult<List<SectionDTO>> result = await studentController.GetSections();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(List<SectionDTO>));
        }

        [TestMethod()]
        public async Task GetStudentTest()
        {            
            ActionResult<StudentDto> result = await studentController.GetStudent(7);
            Assert.IsNotNull(result);
            Assert.AreEqual(200, actual: ((OkObjectResult)result.Result).StatusCode);
        }

        [TestMethod()]
        public async Task DeleteStudentTest()
        {
            ActionResult result = (ActionResult)await studentController.DeleteStudent(7);
            Assert.IsNotNull(result);
            Assert.AreEqual(204, actual: ((NoContentResult)result).StatusCode);
        }
    }
}