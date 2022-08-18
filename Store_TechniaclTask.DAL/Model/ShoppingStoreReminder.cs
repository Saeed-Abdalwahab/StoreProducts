using Store_TechniaclTask.DAL.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store_TechniaclTask.DAL.Model
{
  public  class ShoppingStoreReminder:BaseEntity
    {
        public int ShoppingStoreID { get; set; }
        public DateTime Date { get; set; }
        public bool IsSent { get; set; }
        public virtual ShoppingStore ShoppingStore { get; set; }
    }
}
