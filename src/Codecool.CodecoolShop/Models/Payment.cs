using System.ComponentModel.DataAnnotations;

namespace Codecool.CodecoolShop.Models
{
    public class Payment
    {
        [Required(ErrorMessage = "Please Enter Card Holder Name")]
        [Display(Name = "Full Name")]
        [StringLength(100)]
        public string CardHolder { get; set; }

        [Required(ErrorMessage = "Enter Card Number")]
        [Display(Name = "Card Number")]
        [MaxLength(16)]
        [MinLength(16)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Must be numbers only")]
        public string CardNumber { get; set; }
        
        [Required(ErrorMessage = "Insert Expiry Month")]
        [Display(Name = "Expiry Month")]
        [MaxLength(2)]
        [MinLength(2)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Must be numbers only")]

        public string ExpiryMonth { get; set; }

        [Required(ErrorMessage = "Insert Expiry Year")]
        [Display(Name = "Expiry Year")]
        [MaxLength(2)]
        [MinLength(2)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Must be numbers only")]
        public string ExpiryYear { get; set; }
        [Required(ErrorMessage = "Insert CVV")]
        [Display(Name = "CVV")]
        [MaxLength(3)]
        [MinLength(3)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Must be numbers only")]
        public string CVV { get; set; }
    }
}
