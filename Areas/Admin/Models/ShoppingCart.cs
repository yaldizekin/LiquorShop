using LiquorShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiquorShop.Areas.Admin.Models
{
   
        public class ShoppingCart
        {
            public ShoppingCart()
            {
                Count = 1;
            }

            [Key]
            public int Id { get; set; }
            public string? AppUserId { get; set; }

            [ForeignKey("AppUserId")]
            public AppUser? AppUser { get; set; }
            public int? ProductId { get; set; }

            [ForeignKey("ProductId")]
            public Product? Product { get; set; }
            public int Count { get; set; }

            [NotMapped]
            public double Price { get; set; }
		
	}
    }




