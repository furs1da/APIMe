using APIMe.JwtFeatures;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIMeTests
{
    public class MockJwt
    {
        public static JwtHandler GetJwtHandler()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"JwtSettings", "TopLevelValue"},
                {"JwtSettings:securityKey", "CodeMazeSecretKey"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var mockStore = new Mock<IUserStore<IdentityUser>>();

            var userManager = new Mock<UserManager<IdentityUser>>(mockStore.Object, null, null, null, null, null, null, null, null);
/*            userManager.Setup(p => p.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new IdentityUser { Email = "test", EmailConfirmed = true, Id = "4", UserName = "test" });
            userManager.Setup(p => p.IsEmailConfirmedAsync(It.IsAny<IdentityUser>())).ReturnsAsync(true);
            userManager.Setup(p => p.CheckPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(true);*/
            var list = new List<string>();
            list.Add("test");
            userManager.Setup(p => p.GetRolesAsync(It.IsAny<IdentityUser>())).ReturnsAsync(list);


            JwtHandler jwtHandler = new JwtHandler(configuration, userManager.Object);
            return jwtHandler;
        }
    }
}
