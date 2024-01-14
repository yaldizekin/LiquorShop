using System.ComponentModel.DataAnnotations.Schema;

namespace LiquorShop.Areas.Admin.Models
{
	public class Comment
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime CreatedOn { get; set; }  = DateTime.Now;
		public float Rate { get; set; }
		public int ProductId { get; set; }

		[ForeignKey("ProductId")]
		public virtual Product Product { get; set; }
		public string? AppUserId { get; set; }

		[ForeignKey("AppUserId")]
		public virtual AppUser? User { get; set; }
	
	}
}
