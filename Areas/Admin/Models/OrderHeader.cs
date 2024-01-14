using LiquorShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiquorShop.Areas.Admin.Models
{
    public class OrderHeader 
    {


        public int Id { get; set; }
        public string AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
        public DateTime OrderTime { get; set; } = DateTime.Now;
        public double TotalPrice { get; set; }  
        public string OrderStatus { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Adress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string CardName { get; set; }
        [Required]
        public string CardNo { get; set; }
		[Required]
		public string Cvc { get; set; }
		[Required]
		public string ExpirationYear { get; set; }
		[Required]
		public string ExpirationMonth { get; set; }


	}
}
