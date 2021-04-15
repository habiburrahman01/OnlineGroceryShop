using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingStore.Models
{
    public class Banner
    {
        public string BannerId { get; set; }
        
        [Required]
        [Display(Name = "Banner Image")]
        public string BannerImage { get; set; }

  
    }
}
