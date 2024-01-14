using LiquorShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiquorShop.Areas.Admin.Models
{
	public class Blog 
	{
		public int Id { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public string Summary { get; set; }
        [Required]

        public string Content { get; set; }
        [Required]

        public string Slug { get; set; }

        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string? AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        public AppUser? User { get; set; }



    }
}
