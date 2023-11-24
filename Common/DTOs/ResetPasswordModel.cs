using System.ComponentModel.DataAnnotations;

namespace UserJourney.Common.DTOs
{
    public class ResetPasswordModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(256)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
           
        [Required]
        public string Token { get; set; }
    }
}
