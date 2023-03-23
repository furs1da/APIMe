using System.ComponentModel.DataAnnotations;

namespace APIMe.Entities.DataTransferObjects.Authorization
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? ClientURI { get; set; }
    }
}
