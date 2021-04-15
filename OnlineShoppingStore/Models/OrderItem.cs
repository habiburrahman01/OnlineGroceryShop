using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingStore.Models
{
    public class OrderItem
    {
        public int OrderId { get; set; }

        public string ProductId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "{0:N}")]
        [Display(Name = "Price Per Quantity")]
        public decimal PricePerQuantity { get; set; }

        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "{0:N}")]
        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        public Order Order { get; set; }

        public Product Product { get; set; }
    }
}