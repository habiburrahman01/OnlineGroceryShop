using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingStore.Models
{
    public class CartViewModel
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public int Stock { get; set; }

        public int Quantity { get; set; }

        public decimal PricePerQuantity { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
