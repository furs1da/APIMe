using Microsoft.VisualStudio.TestTools.UnitTesting;
using APIMe.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using APIMe.Entities.Models;
using APIMe.JwtFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using APIMe.Entities.DataTransferObjects.Admin.Section;
using Microsoft.AspNetCore.Mvc;


namespace APIMe.Controllers.Tests
{

    [TestClass()]
    public class SectionControllerTests
    {
        [TestMethod()]
        public void SectionControllerTest()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            IConfiguration configuration = configurationBuilder.Build();
            var mockStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new UserManager<IdentityUser>(mockStore.Object, null, null, null, null, null, null, null, null);
            APIMeContext aPIMeContext = new APIMeContext();
            JwtHandler jwtHandler = new JwtHandler(configuration, userManager);
            SectionController sectionController = new SectionController(userManager, aPIMeContext, jwtHandler);
            Assert.IsNotNull(sectionController);
        }

        [TestMethod()]
        public async Task GetSectionsTestAsync()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            IConfiguration configuration = configurationBuilder.Build();
            var mockStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new UserManager<IdentityUser>(mockStore.Object, null, null, null, null, null, null, null, null);
            APIMeContext aPIMeContext = new APIMeContext();
            JwtHandler jwtHandler = new JwtHandler(configuration, userManager);
            SectionController sectionController = new SectionController(userManager, aPIMeContext, jwtHandler);
            ActionResult<List<SectionDTO>> result = await sectionController.GetSections();
            Assert.IsNotNull(result);
        }
        [TestMethod()]
        public async Task AddSectionTest()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            IConfiguration configuration = configurationBuilder.Build();
            var mockStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new UserManager<IdentityUser>(mockStore.Object, null, null, null, null, null, null, null, null);
            APIMeContext aPIMeContext = new APIMeContext();
            JwtHandler jwtHandler = new JwtHandler(configuration, userManager);
            SectionController sectionController = new SectionController(userManager, aPIMeContext, jwtHandler);
            SectionDTO section=new SectionDTO();
            section.Id = 40000;
            section.SectionName = "Test";
            section.NumberOfStudents = 1;
            section.ProfessorName = "Test";
            section.AccessCode = "Test";
            ActionResult<SectionDTO> result = await sectionController.AddSection(section);
            Assert.IsNotNull(result);
            Assert.Fail();
        }

        [TestMethod()]
        public async Task EditSectionTestAsync()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            IConfiguration configuration = configurationBuilder.Build();
            var mockStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new UserManager<IdentityUser>(mockStore.Object, null, null, null, null, null, null, null, null);
            APIMeContext aPIMeContext = new APIMeContext();
            JwtHandler jwtHandler = new JwtHandler(configuration, userManager);
            SectionController sectionController = new SectionController(userManager, aPIMeContext, jwtHandler);
            SectionDTO section = new SectionDTO();
            section.Id = 40000;
            section.SectionName = "Test";
            section.NumberOfStudents = 1;
            section.ProfessorName = "Test";
            section.AccessCode = "Test";
            ActionResult<SectionDTO> result = await sectionController.EditSection(40000,section);
            Assert.IsNotNull(result);
            Assert.Fail();
        }

        [TestMethod()]
        public async Task DeleteSectionTestAsync()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            IConfiguration configuration = configurationBuilder.Build();
            var mockStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new UserManager<IdentityUser>(mockStore.Object, null, null, null, null, null, null, null, null);
            APIMeContext aPIMeContext = new APIMeContext();
            JwtHandler jwtHandler = new JwtHandler(configuration, userManager);
            SectionController sectionController = new SectionController(userManager, aPIMeContext, jwtHandler);
            SectionDTO section = new SectionDTO();
            section.Id = 40000;
            section.SectionName = "Test";
            section.NumberOfStudents = 1;
            section.ProfessorName = "Test";
            section.AccessCode = "Test";
            IActionResult result = await sectionController.DeleteSection(40000);
            Assert.IsNotNull(result);
            Assert.Fail();
        }

        [TestMethod()]
        public void PrivacyTest()
        {
            Assert.Fail();
        }
    }
}