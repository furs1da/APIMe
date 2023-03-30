﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIMe.Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace APIMeTests
{
        public class MockContext<T>
        {
            public APIMeContext GetMockContext()
            {
                var options = new DbContextOptionsBuilder<APIMeContext>()
                          .UseInMemoryDatabase(Guid.NewGuid().ToString())
                          .Options;
                var context = new APIMeContext(options);

                context.Sections.Add(new Section { Id = 4, SectionName = "n/a", ProfessorId = 0, AccessCode = "n/a" });
                context.UserClaims.Add(new IdentityUserClaim<string> { ClaimType="", ClaimValue="", Id=0, UserId="0"});
                context.Professors.Add(new Professor { Id = 0, Email = "testingmockup", FirstName = "first", LastName = "last" });
                context.Users.Add(new IdentityUser { Email ="test", EmailConfirmed= true, Id="0"  });
                context.Students.Add(new Student { Id = 7, FirstName = "test", LastName = "test", StudentId = 1234567, Email = "test", ApiKey = "t" });
                context.StudentSections.Add(new StudentSection { SectionId = 4, StudentId = 7 });
            context.Routes.Add(new Route { Id = 1, Name = "testRoute", Description = "describe", IsVisible = true, DataTableName = "Products", RouteTypeId = 1 });
                context.RouteTypes.Add(new RouteType { Id=1,CrudId=1, Name="GET", ResponseCode="201" });

                context.SaveChanges();
                return context;
            }
        }
    }