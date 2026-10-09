using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Request
{
    public class ResetPasswordRequest
    {
        [Required(ErrorMessage = "Reset token is required.")] 
        public string Token { get; set; } = string.Empty; 

        [Required(ErrorMessage = "New password is required.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{8,}$", 
        ErrorMessage = "Password must contain at least 8 characters, including uppercase, lowercase, number, and " +
        "special character.")] 
        public string NewPassword { get; set; } = string.Empty; 


        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")] 
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
