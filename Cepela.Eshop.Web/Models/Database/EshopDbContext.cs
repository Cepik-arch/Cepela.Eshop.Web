using Cepela.Eshop.Web.Models.Database.Configurations;
using Cepela.Eshop.Web.Models.Entity;
using Cepela.Eshop.Web.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cepela.Eshop.Web.Models.Database
{
    public class EshopDbContext : IdentityDbContext<User,Role,int>
    {

        public DbSet<CarouselItem> CarouselItems { get; set; }

        public DbSet<ProductItem> ProductItems { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public EshopDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration<Order>(new OrderConfigurations());

            foreach(var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName().Replace("AspNet", string.Empty));
            }
        }
    }
}
