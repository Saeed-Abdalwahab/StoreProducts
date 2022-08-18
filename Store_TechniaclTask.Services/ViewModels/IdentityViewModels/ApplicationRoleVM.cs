using Store_TechniaclTask.DAL.Enums;
using Store_TechniaclTask.Services.ModelServices.Abstraction.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Store_TechniaclTask.Services.ViewModels.IdentityViewModels
{
   public class ApplicationRoleVM:IValidatableObject
    {
        public ApplicationRoleVM()
        {
            RoleClaims = new List<RoleClaim>();
            
        }
        public string ID { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        public List<RoleClaim> RoleClaims { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var repo = (IApplicationRoleService)validationContext.GetService(typeof(IApplicationRoleService));
            var validation = repo._ValidationResult(this);
            return validation;
        }
    }

    public class RoleClaim
    {
        public RoleClaim()
        {

            //SelectedPermissions = new List<SharedPermissions>();
            //ClaimValue = JsonSerializer.Serialize(SelectedPermissions.Select(x=>(int)x));

        }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        //public List<SharedPermissions> SelectedPermissions { get; set; }


    }
}
