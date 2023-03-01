using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using APIMe.Data;
using Microsoft.Extensions.Options;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Authorization;
using APIMe.Models;
using APIMe.Interfaces;

namespace APIMe.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
      
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private readonly IMailService mailService;
       
        private APIMe.Models.APIMeContext _apiContext;


        public AccountController(UserManager<User> userMngr, SignInManager<User> signInMngr, IMailService mailService)
        {
         
        }

    }
}