using LiquorShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiquorShop.Areas.Admin.Models
{
	public class Product 
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }
		public bool IsActive { get; set; } = false;
		public DateTime CreatedOn { get; set; } = DateTime.Now;
		public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public string? ImagePath { get; set; }
		
        [NotMapped]
        public IFormFile? Image { get; set; }
        public Category? Category { get; set; }
		public int? CategoryId { get; set; }

        public string? AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        public AppUser? User { get; set; }
		public virtual ICollection<Comment> Comment { get; set; }


	}
}
