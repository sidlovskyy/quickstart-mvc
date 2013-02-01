using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace Logfox.Web.UI.Models.Account
{
    public class UserViewModel
    {
        [Required]
        [DisplayName("Full Name")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Company")]
        public string Company { get; set; }

        [Required]
        [Email]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required]
        [DisplayName("Password Confirmation")]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
    }
}