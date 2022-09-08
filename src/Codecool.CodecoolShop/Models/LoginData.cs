using System.ComponentModel.DataAnnotations;

namespace Codecool.CodecoolShop.Models
{
    public class LoginData
    {
        [Required(ErrorMessage = "Please enter your User name")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your Password")]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
