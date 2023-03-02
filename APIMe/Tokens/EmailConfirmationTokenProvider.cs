using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace APIMe.Tokens
{
    public class EmailConfirmationTokenProvider<TUser> : DataProtectorTokenProvider<TUser>
        where TUser : class
    {
        public EmailConfirmationTokenProvider(IDataProtectionProvider dataProtectionProvider,
            IOptions<DataProtectionTokenProviderOptions> options,
            ILogger<DataProtectorTokenProvider<TUser>> logger)
            : base(dataProtectionProvider, options, logger)
        {
        }

        public override async Task<string> GenerateAsync(string purpose,
            UserManager<TUser> manager, TUser user)
        {
            return await base.GenerateAsync(purpose, manager, user);
        }

        public override async Task<bool> ValidateAsync(string purpose, string token,
            UserManager<TUser> manager, TUser user)
        {
            return await base.ValidateAsync(purpose, token, manager, user);
        }

        public override async Task<bool> CanGenerateTwoFactorTokenAsync(
            UserManager<TUser> manager, TUser user)
        {
            return false;
        }
    }

    public class EmailConfirmationTokenProviderOptions : DataProtectionTokenProviderOptions
    {
    }
}
