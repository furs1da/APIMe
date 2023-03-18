using System.ComponentModel.DataAnnotations;

namespace APIMe.Entities.DataTransferObjects
{
    public class UserForRegistrationDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Student Number is required.")]
        public string? StudentNumber { get; set; }

        [Required(ErrorMessage = "Student Section is required.")]
        public int? StudentSection { get; set; }

        [Required(ErrorMessage = "Access Code is required.")]
        public string? AccessCode { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        public string? ClientURI { get; set; }
    }
}
