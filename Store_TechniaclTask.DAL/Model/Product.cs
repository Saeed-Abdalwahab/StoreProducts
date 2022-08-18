using Microsoft.EntityFrameworkCore;
using Store_TechniaclTask.DAL.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store_TechniaclTask.DAL.Model
{
    public class Product : BaseEntity, ISoftDeletedEntity
    {
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        //public DateTime lastUpdateDate { get; set; }
        public static void ConfigreUniqKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasIndex(p => new
            {
                p.Name,
            }).IsUnique();

        }
    }
}
