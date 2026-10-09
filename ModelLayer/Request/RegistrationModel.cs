using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Request
{
    public class RegistrationModel
    {
        [Required(AllowEmptyStrings =false)]
        [StringLength(50, MinimumLength =3, ErrorMessage = "First name must be between 3 and 50 characters")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Last name must be between 3 and 50 characters")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[@#!$%^&*()_\-+]).{8,}$",
        ErrorMessage = "Password should contain at least 8 characters, one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage ="Contact Number is required")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage= "Contact number must be a valid 10-digit Indian mobile number")]
        public string ContactNo { get; set; }

    }
}
