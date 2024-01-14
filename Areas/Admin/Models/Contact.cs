using LiquorShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiquorShop.Areas.Admin.Models
{

	public class Contact
	{
		public int Id { get; set; }

		[Required ]
		public string FullName { get; set; }
		[Required]

		public string Email { get; set; }
		[Required]

		public string Subject { get; set; }
		[Required]

		public string Message { get; set; }
		public DateTime CreatedOn { get; set; }
        public string? AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        public AppUser? User { get; set; }
    }
}
