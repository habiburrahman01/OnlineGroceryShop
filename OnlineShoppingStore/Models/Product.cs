using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingStore.Models
{
    public class Product
    {
        public string ProductId { get; set; }

        [Required]
        [Display(Name ="Name")]
        public string ProductName { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Image")]
        public string ProductImage { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "{0:N}")]
        public decimal Price { get; set; }

        public Category Category { get; set; }
    }
}
