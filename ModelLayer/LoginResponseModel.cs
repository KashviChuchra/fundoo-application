using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer
{
    public class LoginResponseModel
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
