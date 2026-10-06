using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelLayer
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage ="Email is Required")]
        [EmailAddress(ErrorMessage ="Email is not valid")]
        public string Email { get; set; } = string.Empty;
    }
}

// data user will send