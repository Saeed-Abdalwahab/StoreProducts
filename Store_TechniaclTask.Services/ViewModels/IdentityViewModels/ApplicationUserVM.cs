using Store_TechniaclTask.Services.HelperServices;
using Store_TechniaclTask.Services.ModelServices.Abstraction.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Store_TechniaclTask.DAL.Enums;

namespace Store_TechniaclTask.Services.ViewModels.IdentityViewModels
{

   
    public class ApplicationUserVM : ApplicationUserPasswordDTO,  IValidatableObject
    {
        public ApplicationUserVM()
        {
        }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Name")]
      
        public string FirstName { get; set; }
       
        [Display(Name = "UserName")]
        
        public virtual string UserName { get; set; }
        [Display(Name = "Email")]
        
        [EmailAddress(ErrorMessage = "InvalidEmail")]
        [Required(ErrorMessage = "Required")]
        public string Email { get; set; }
        [Display(Name = "_Role")]
        public string Role { get; set; }
        public bool EmailConfirmed { get; set; } = true;
        [Display(Name = "PhoneNumber")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{5})$", ErrorMessage = "InValidMobile")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public virtual  IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var repo = (IApplicationUserService)validationContext.GetService(typeof(IApplicationUserService));
            var validation = repo._ValidationResult(this);
            return validation;
        }
    }
    public class ApplicationUserPasswordDTO
    {

        [StringLength(100, ErrorMessage = "MustbeAtleast_characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
         public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessage = "PasswordAndConfirmPasswordNotMatched")]
        public string ConfirmPassword { get; set; }
        public string ID { get; set; }
    }

}
