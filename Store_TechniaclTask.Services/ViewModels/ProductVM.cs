using Store_TechniaclTask.DAL.Helper;
using Store_TechniaclTask.Services.ModelProducts.Abstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Store_TechniaclTask.Services.ViewModels
{
    public class ProductVM : IValidatableObject
    {
        public ProductVM()
        {
        }
        public int ID { get; set; }
        public bool IsDeleted { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Price")]
        [Range(0, double.MaxValue, ErrorMessage = "InvalidNumber")] 
        public double Price { get; set; }
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
         
            var repo = (IProductService)validationContext.GetService(typeof(IProductService));
            var validation = repo._ValidationResult(this);

            return validation;
        }
    }
}
