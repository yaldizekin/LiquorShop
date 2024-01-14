using LiquorShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiquorShop.Areas.Admin.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }

        public ICollection<Blog>? Blogs { get; set; }
        public ICollection<Category>? Categories { get; set; }
        public ICollection<Product>? Products { get; set; }
        public ICollection<Contact>? Contacts { get; set; }
		public virtual ICollection<Comment> Comment { get; set; }

		[NotMapped]
        public string? Role { get; set; }




    }
}
