using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelLayer.Request
{
    public class ForgotPasswordRequest
    {
        [Required(ErrorMessage ="Email is Required")]
        [EmailAddress(ErrorMessage ="Email is not valid")]
        public string Email { get; set; } = string.Empty;
    }
}

// data user will send