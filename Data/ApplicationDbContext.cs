using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using LiquorShop.Areas.Admin.Models;

namespace LiquorShop.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<LiquorShop.Areas.Admin.Models.Blog>? Blogs { get; set; }
        public DbSet<LiquorShop.Areas.Admin.Models.Category>? Categories { get; set; }
        public DbSet<LiquorShop.Areas.Admin.Models.Product>? Products { get; set; }
        public DbSet<LiquorShop.Areas.Admin.Models.Contact>? Contacts { get; set; }
        public DbSet<LiquorShop.Areas.Admin.Models.AppUser>? AppUser { get; set; }    
        public DbSet<LiquorShop.Areas.Admin.Models.Comment>? Comments { get; set; }    
        public DbSet<LiquorShop.Areas.Admin.Models.ShoppingCart>? ShoppingCart { get; set; }
        public DbSet<LiquorShop.Areas.Admin.Models.OrderHeader>? OrderHeader { get; set; }
        public DbSet<LiquorShop.Areas.Admin.Models.OrderDetail>? OrderDetail { get; set; }

    }
}