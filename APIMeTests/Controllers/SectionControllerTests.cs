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
        public APIMeContext? ContextDataAccess { get; set; }

        [SetUp]
        public void TestMethodSetup()
        {
            this.ContextDataAccess = Substitute.For<APIMeContext>();
            this.ContextDataAccess = new MockContext<APIMeContext>().GetMockContext();
        }

        [TestMethod()]
            public void SectionControllerTest()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            IConfiguration configuration = configurationBuilder.Build();
            var mockStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new UserManager<IdentityUser>(mockStore.Object, null, null, null, null, null, null, null, null);
            JwtHandler jwtHandler = new JwtHandler(configuration, userManager);
            this.ContextDataAccess = new MockContext<APIMeContext>().GetMockContext();
            SectionController sectionController = new SectionController(userManager, ContextDataAccess, jwtHandler);
            Assert.IsNotNull(sectionController);
        }

        [TestMethod()]
            public async Task GetSectionsTest()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            IConfiguration configuration = configurationBuilder.Build();
            var mockStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new UserManager<IdentityUser>(mockStore.Object, null, null, null, null, null, null, null, null);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, "TestingUser"),
                                        new Claim(ClaimTypes.Name, "test@test.com")
                                   }, "TestAuthentication"));
            JwtHandler jwtHandler = new JwtHandler(configuration, userManager);
            this.ContextDataAccess = new MockContext<APIMeContext>().GetMockContext();
            SectionController sectionController = new SectionController(userManager, ContextDataAccess, jwtHandler);
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
            JwtHandler jwtHandler = new JwtHandler(configuration, userManager);         
            this.ContextDataAccess = new MockContext<APIMeContext>().GetMockContext();
            SectionController sectionController = new SectionController(userManager, ContextDataAccess, jwtHandler);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { 
                new Claim(ClaimTypes.Name, "testingmockup"),
            }, "mock"));
            sectionController.ControllerContext.HttpContext = new DefaultHttpContext() { User = user };
            SectionDTO section=new SectionDTO();
            section.Id = 1;
            section.SectionName = "Test";
            section.NumberOfStudents = 1;
            section.ProfessorName = "Test";
            section.AccessCode = "Test";
            ActionResult<SectionDTO> result = await sectionController.AddSection(section);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
            public async Task EditSectionTest()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            IConfiguration configuration = configurationBuilder.Build();
            var mockStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new UserManager<IdentityUser>(mockStore.Object, null, null, null, null, null, null, null, null);
            JwtHandler jwtHandler = new JwtHandler(configuration, userManager);
            this.ContextDataAccess = new MockContext<APIMeContext>().GetMockContext();
            SectionController sectionController = new SectionController(userManager, ContextDataAccess, jwtHandler);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testingmockup"),
            }, "mock"));
            sectionController.ControllerContext.HttpContext = new DefaultHttpContext() { User = user };
            SectionDTO section = new SectionDTO();
            section.Id = 4;
            section.SectionName = "Test";
            section.NumberOfStudents = 1;
            section.ProfessorName = "Test";
            section.AccessCode = "Test";
            ActionResult<SectionDTO> result = await sectionController.EditSection(4,section);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
            public async Task DeleteSectionTest()
        {
            NoContentResult expected=new();
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            IConfiguration configuration = configurationBuilder.Build();
            var mockStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new UserManager<IdentityUser>(mockStore.Object, null, null, null, null, null, null, null, null);          
            JwtHandler jwtHandler = new JwtHandler(configuration, userManager);
            this.ContextDataAccess = new MockContext<APIMeContext>().GetMockContext();

            SectionController sectionController = new SectionController(userManager, ContextDataAccess, jwtHandler); 
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {              
                new Claim(ClaimTypes.Name, "testingmockup"),
            }, "mock"));
            sectionController.ControllerContext.HttpContext = new DefaultHttpContext() { User = user };
            IActionResult result = await sectionController.DeleteSection(4);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
            public void PrivacyTest()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            IConfiguration configuration = configurationBuilder.Build();
            var mockStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new UserManager<IdentityUser>(mockStore.Object, null, null, null, null, null, null, null, null);
            JwtHandler jwtHandler = new JwtHandler(configuration, userManager);
            this.ContextDataAccess = new MockContext<APIMeContext>().GetMockContext();
            SectionController sectionController = new SectionController(userManager, ContextDataAccess, jwtHandler);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testingmockup"),
            }, "mock"));
            sectionController.ControllerContext.HttpContext = new DefaultHttpContext() { User = user };
            IActionResult result = sectionController.Privacy();
            Assert.IsNotNull(result);
        }
    }
}