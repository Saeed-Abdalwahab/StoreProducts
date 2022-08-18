using Newtonsoft.Json;
using Store_TechniaclTask.DAL.Enums;
using Store_TechniaclTask.Services.CustomAttribute;
using Store_TechniaclTask.Services.ModelServices.Abstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Store_TechniaclTask.Services.ViewModels
{
  public  class ShoppingStoreVM : IValidatableObject
    {
        public ShoppingStoreVM()
        {
            DetailsVM = new List<ShoppingStoreDetailsVM>();
            RegistrationDate = DateTime.Now;
        }
        public int ID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public double TotalPrices { get; set; }
        public ShoppingStoreStatus ShoppingStoreStatus { get; set; }
        public DateTime RegistrationDate { get; set; }
        [MinDateValidation(false,ErrorMessage ="valid min Date is ")]
        public DateTime? ReminderTime { get; set; }
        public string RegistrationDate_str { get { return RegistrationDate.ToString("dddd, dd MMMM yyyy HH:mm:ss"); } }
        public List<ShoppingStoreDetailsVM> DetailsVM { get; set; }
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            var repo = (IShoppingStoreService)validationContext.GetService(typeof(IShoppingStoreService));
            var validation = repo._ValidationResult(this);

            return validation;
        }
    }
}
