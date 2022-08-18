using Microsoft.EntityFrameworkCore;
using Store_TechniaclTask.DAL.Enums;
using Store_TechniaclTask.DAL.Helper;
using Store_TechniaclTask.DAL.Model.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store_TechniaclTask.DAL.Model
{
    public class ShoppingStore : BaseEntity
    {
        public ShoppingStore()
        {
            Details = new HashSet<ShoppingStoreDetails>();
            ShoppingStoreReminders = new HashSet<ShoppingStoreReminder>();
        }
        public string UserID { get; set; }
        public DateTime RegistrationDate { get; set; }
        public ShoppingStoreStatus ShoppingStoreStatus { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<ShoppingStoreDetails> Details { get; set; }
        public virtual ICollection<ShoppingStoreReminder> ShoppingStoreReminders { get; set; }
        public static void ConfigreUniqKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingStore>().HasMany(b => b.Details).WithOne(p => p.ShoppingStore)
    .HasForeignKey(p => p.ShoppingStoreID)
    .OnDelete(DeleteBehavior.Cascade);  
            modelBuilder.Entity<ShoppingStore>().HasMany(b => b.ShoppingStoreReminders).WithOne(p => p.ShoppingStore)
    .HasForeignKey(p => p.ShoppingStoreID)
    .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ShoppingStoreDetails>().HasIndex(p => new
            {
                p.ProductID,
                p.ShoppingStoreID,
            }).IsUnique();
        }
    }
}
