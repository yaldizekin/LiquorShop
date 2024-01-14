using LiquorShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiquorShop.Areas.Admin.Models
{
	public class Category
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public ICollection <Product>? Products { get; set;}
        public string? AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        public AppUser? User { get; set; }

    }
}
