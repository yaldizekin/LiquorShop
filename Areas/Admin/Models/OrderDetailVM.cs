using LiquorShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiquorShop.Areas.Admin.Models
{
    public class OrderDetailVM
    {

       public OrderHeader OrderHeader { get; set; }
        
        public IEnumerable<OrderDetail> OrderDetail { get; set; }

    }
}
