using Store_TechniaclTask.DAL.Model;
using Store_TechniaclTask.DAL.Model.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Store_TechniaclTask.DAL.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>,
    ApplicationUserRole, IdentityUserLogin<string>,
    ApplicationRoleClaim, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.SetCommandTimeout(TimeSpan.FromMinutes(5));
        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ShoppingStore> ShoppingStores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Set All Relation On Delete No Action
            base.OnModelCreating(modelBuilder);
            //IDentity 
            modelBuilder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
            ConfigreUniqKeys(modelBuilder);

            defaultValues(modelBuilder);
        }
        void ConfigreUniqKeys(ModelBuilder modelBuilder)
        {
            ApplicationRoleClaim.ConfigreUniqKeys(modelBuilder);
          
           
            ApplicationUserRole.ConfigreUniqKeys(modelBuilder);
            Product.ConfigreUniqKeys(modelBuilder);
            ShoppingStore.ConfigreUniqKeys(modelBuilder);
            Product.ConfigreUniqKeys(modelBuilder);
        }
        void defaultValues(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().Property(b => b.RegistrationDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<ApplicationUser>().Property(b => b.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<ApplicationRole>().Property(b => b.Id) .HasDefaultValueSql("NEWID()");
        }
    }
}




