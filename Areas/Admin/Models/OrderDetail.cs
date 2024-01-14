using LiquorShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiquorShop.Areas.Admin.Models
{
    public class OrderDetail
    {

        public int Id { get; set; }
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public OrderHeader OrderHeader { get; set; }
        public int? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }

    }
}
