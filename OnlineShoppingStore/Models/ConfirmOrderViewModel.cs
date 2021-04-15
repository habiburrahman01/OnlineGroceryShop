using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingStore.Models
{
    public class ConfirmOrderViewModel
    {

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^?([0−9]3)?([0-9]{3})?([0−9]3)?([0-9]{8})$", ErrorMessage = "It's not a valid phone number")]
        [Display(Name = "Mobile number")]
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Column(TypeName = "money")]
        [Display(Name = "Shipping Charge")]
        public decimal ShippingCharge { get; set; }

        [Required]
        [Column(TypeName = "money")]
        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        public Payment Payment { get; set; }

        public ICollection<CartViewModel> CartItems { get; set; }
    }
}
