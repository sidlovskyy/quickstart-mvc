using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace Logfox.Web.UI.Models.ContactUs
{
    public class ContactUsViewModel : BaseViewModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Your Name")]
        public string SenderName { get; set; }

        [Required]
        [Email]
        [StringLength(200)]
        [Display(Name = "Email")]
        public string SenderEmail { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required]
        [StringLength(50000)]
        [Display(Name = "Message")]
        public string Message { get; set; }       
    }
}