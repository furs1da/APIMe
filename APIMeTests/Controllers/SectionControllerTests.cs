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
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using System;
using System.Collections.Generic;
using NSubstitute;
using APIMeTests;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Authorization;
using APIMe.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using static Duende.IdentityServer.Models.IdentityResources;
using System.IdentityModel.Tokens.Jwt;
using APIMe.Entities.DataTransferObjects;
using APIMe.Entities.DataTransferObjects.Authorization;
using Duende.IdentityServer.Models;
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


/*        private void testSetup()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            IConfiguration configuration = configurationBuilder.Build();
            var mockStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new UserManager<IdentityUser>(mockStore.Object, null, null, null, null, null, null, null, null);
            JwtHandler jwtHandler = new JwtHandler(configuration, userManager);
            this.ContextDataAccess = new MockContext<APIMeContext>().GetMockContext();

            controller = new SectionController(userManager, ContextDataAccess, jwtHandler);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testingmockup"),
            }, "mock"));
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = user };
        }*/
    }
}