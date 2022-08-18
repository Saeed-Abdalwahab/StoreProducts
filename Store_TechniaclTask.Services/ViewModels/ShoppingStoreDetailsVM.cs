using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Store_TechniaclTask.Services.ViewModels
{
    public class ShoppingStoreDetailsVM
    {
        public int ID { get; set; }
       
        public int ShoppingStoreID { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Name")]
        public int ProductID { get; set; }
        public string ProductName { get; set; }
       
        public double ProductPrice { get; set; }
    }
}
