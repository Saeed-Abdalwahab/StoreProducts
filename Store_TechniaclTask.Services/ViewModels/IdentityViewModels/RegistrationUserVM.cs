//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Text;

//namespace Store_TechniaclTask.Services.ViewModels.IdentityViewModels
//{
//    public class RegistrationUserVM
//    {
//        public string Id { get; set; }
//        [Required(ErrorMessage = "Required")]
//        public string FirstName { get; set; }
//        public string LastName { get; set; }
//        public string UserName { get; set; }
//        [Display(Name = "Email")]
//        [EmailAddress(ErrorMessage = "InvalidEmail")]
//        [Required(ErrorMessage = "Required")]
//        public string Email { get; set; }
//        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
//        [DataType(DataType.Password)]
//        [Display(Name = "Password")]
//        [Required(ErrorMessage = "Required")]
//        public string Password { get; set; }
//        [DataType(DataType.Password)]
//        [Display(Name = "ConfirmPassword")]
//        [Compare("Password", ErrorMessage = "PasswordAndConfirmPasswordNotMatched")]
//        public string ConfirmPassword { get; set; }
//        public bool EmailConfirmed { get; set; } = true;
//        public string PhoneNumber { get; set; }
//    }
//}
