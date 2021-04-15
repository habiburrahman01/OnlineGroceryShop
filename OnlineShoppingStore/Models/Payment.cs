using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingStore.Models
{
    public class Payment
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        [Display(Name = "Payment Method ")]
        public PaymentType Type { get; set; }

        [Phone]
        public string BkashNo { get; set; }

        public Order Order { get; set; }
    }
    public enum PaymentType
    {
        Cash_On_Delivery,
        BKash
    }
}