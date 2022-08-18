using Microsoft.EntityFrameworkCore;
using Store_TechniaclTask.DAL.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store_TechniaclTask.DAL.Model
{
   public class ShoppingStoreDetails:BaseEntity
    {
        public int ShoppingStoreID { get; set; }
        public int ProductID { get; set; }
        public double ProductPrice { get; set; }
        public virtual Product Product { get; set; }
        public virtual ShoppingStore ShoppingStore { get; set; }
      

    }
}
