using System.ComponentModel.DataAnnotations;

namespace MessagingService_ModelLayer
{
    public class EmailModel
    {
        [Required (ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage ="Email is invalid")]
        public string ToEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subject is required")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Body is required")]
        public string Body { get; set; } = string.Empty;
    }
}
