using APIMe.JwtFeatures;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

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

            var list = new List<string>
            {
                "test"
            };
            userManager.Setup(p => p.GetRolesAsync(It.IsAny<IdentityUser>())).ReturnsAsync(list);


            JwtHandler jwtHandler = new JwtHandler(configuration, userManager.Object);
            return jwtHandler;
        }
    }
}
