using System.ComponentModel.DataAnnotations;

namespace UserJourney.Common.DTOs
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "First Name is required.")]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(256)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
