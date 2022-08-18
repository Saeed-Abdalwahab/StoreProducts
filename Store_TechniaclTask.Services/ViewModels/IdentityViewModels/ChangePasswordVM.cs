using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Store_TechniaclTask.Services.ViewModels.IdentityViewModels
{
   public class ChangePasswordVM: ApplicationUserPasswordDTO
    {
        [DataType(DataType.Password)]
        [Display(Name = "OldPassword")]
        public string CurrentPassword { get; set; }
    }   
    public class ResetPassword: ApplicationUserPasswordDTO
    {
 
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
