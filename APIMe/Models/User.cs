using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIMe.Models
{
    public class User : IdentityUser
    {
        [NotMapped]
        public string? RoleName { get; set; }
    }
}
