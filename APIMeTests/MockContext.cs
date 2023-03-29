﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIMe.Entities.Models;

namespace APIMeTests
{
        /// <summary>
        /// Context Entity Mock
        /// </summary>
        /// <typeparam name="T">Fake entity context</typeparam>
        public class MockContext<T>
        {
            /// <summary>
            /// Get  context mock object
            /// </summary>
            /// <returns>Context object</returns>
            public APIMeContext GetMockContext()
            {
                var options = new DbContextOptionsBuilder<APIMeContext>()
                          .UseInMemoryDatabase(Guid.NewGuid().ToString())
                          .Options;
                var context = new APIMeContext(options);

                // Sections => table name
                context.Sections.Add(new Section { Id = 4, SectionName = "n/a", ProfessorId = 0, AccessCode = "n/a" });
                context.UserClaims.Add(new Microsoft.AspNetCore.Identity.IdentityUserClaim<string> { ClaimType="", ClaimValue="", Id=0, UserId="0"});
                context.Professors.Add(new Professor { Id = 0, Email = "testingmockup", FirstName = "first", LastName = "last" });

                context.SaveChanges();
                return context;
            }
        }
    }