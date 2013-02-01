using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QuickStartProject.Web.UI.Models.Account
{
    public class AccountViewModel
    {
        [Required]
        [DisplayName("User name")]
        public string Username { get; set; }

        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}