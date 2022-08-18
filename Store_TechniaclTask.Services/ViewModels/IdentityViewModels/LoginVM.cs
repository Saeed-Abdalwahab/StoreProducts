using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Store_TechniaclTask.Services.ViewModels.IdentityViewModels
{
    public class LoginVM
    {
        [Display(Name = "UserName/Email")]
        [Required(ErrorMessage = "Required")]

        public string UserName { get; set; }
        [Required(ErrorMessage ="Required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "RememberMe")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
