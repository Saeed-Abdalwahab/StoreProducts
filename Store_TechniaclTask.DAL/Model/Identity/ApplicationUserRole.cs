using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store_TechniaclTask.DAL.Model.Identity
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }

        public static void ConfigreUniqKeys(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<UserFCMToken>().HasIndex(p => new
            //{
            //    //p.Token,
            //}).IsUnique();
            modelBuilder.Entity<ApplicationUserRole>().HasOne(b => b.User).WithMany(p => p.UserRoles)
    .HasForeignKey(p => p.UserId)
    .OnDelete(DeleteBehavior.Cascade);   
            modelBuilder.Entity<ApplicationUserRole>().HasOne(b => b.Role).WithMany(p => p.UserRoles)
    .HasForeignKey(p => p.RoleId)
    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
