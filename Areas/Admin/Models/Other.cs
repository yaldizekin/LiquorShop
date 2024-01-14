using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiquorShop.Areas.Admin.Models
{
    public static class Other
    {

        public const string Role_Person = "Person";
        public const string Role_Admin = "Admin";
        public const string Role_User = "User";

        public const string ShoppingCartSs = "Shopping Cart Session ";


        public const string Approved = "Approved";
        public const string Pending = "Pending";
        public const string Shipped = "Shipped";
    }
}
