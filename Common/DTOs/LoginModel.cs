using System.ComponentModel.DataAnnotations;

namespace UserJourney.Common.DTOs
{
    public class LoginModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(256)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
