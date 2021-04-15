using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingStore.Models
{
    public class Shipping
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public string Shipper { get; set; }

        [Required]
        [Phone]
        [RegularExpression(@"^?([0−9]3)?([0-9]{3})?([0−9]3)?([0-9]{8})$", ErrorMessage = "It's not a valid phone number")]
        public string Phone { get; set; }

        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "{0:N}")]
        public decimal Charge { get; set; }

        public Order Order { get; set; }
    }
}