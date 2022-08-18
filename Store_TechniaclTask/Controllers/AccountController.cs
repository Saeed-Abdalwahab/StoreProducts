using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Store_TechniaclTask.Services.ModelServices.Abstraction.Identity;
using Store_TechniaclTask.Services.Resources;
using Store_TechniaclTask.Services.ViewModels.IdentityViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store_TechniaclTask.DAL.Model.Identity;
using Store_TechniaclTask.Services.ViewModels.DTO;
using Store_TechniaclTask.Services.HelperServices;

namespace _Store_TechniaclTask.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IApplicationUserService applicationUserService;
        private readonly LocService locservice;

        public AccountController(IApplicationUserService applicationUserService, LocService locservice)
        {
            this.applicationUserService = applicationUserService;
            this.locservice = locservice;
        }
        public IActionResult login(string ReturnUrl)
        {
            //ViewBag.ReturnUrl = ReturnUrl;
            return View(new LoginVM() { ReturnUrl = ReturnUrl });
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            // If password reset token or email is null, most likely the
            // user tried to tamper the password reset link
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View(new ResetPassword { Token = token, Email = email });
        }
        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await applicationUserService.IsValidUserAsync(new LoginVM { UserName = model.Email });
                if (user.User!=null)
                {
                    // reset the user password
                    var result = await applicationUserService.ChangePassword(user.User, model);
                    if (result.Status)
                    {
                        return await login(new LoginVM { UserName = model.Email, Password = model.Password });
                    }
                    // Display validation errors. For example, password reset token already
                    // used to change the password or password complexity rules not met
                    ModelState.AddModelError("", result.Message);
                    return View(model);
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist
                return RedirectToAction("NotFound");
            }
            // Display validation errors if model state is not valid
            return View(model);
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var User = await applicationUserService.IsValidUserAsync(new LoginVM { UserName = email });
            if (User.User==null)
            {
                ModelState.AddModelError("", "not Valid User");
                return View();
            }
            var token =await applicationUserService.GeneratePassResetToken(User.User);

           
                var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = email, token = token }, Request.Scheme);

                var Body = $@"<!DOCTYPE html><html lang='en'><head><meta charset='UTF-8' />
                 <meta http-equiv='X-UA-Compatible' content='IE=edge' />
                 <meta name='viewport' content='width=device-width, initial-scale=1.0' /> 
                 <title>Change password</title></head><body style='margin: 3px'><div class='container' style='text-align: center; background-color: #fbf4e4; height: 110vh'><div class='body-email'
style='position: absolute;top: 50%; left: 50%; transform: translate(-40%, -40%);'>
                 <div class='title'><h2 style='margin:
3px'>Friendzr</h2></div><h1>Let's Change password</h1><div
style='font-size: 17px'>  We got a request to change password
for your  account  .</div><div class='code'style='font-size:
20px; font-weight: bold; margin: 17px'></br> 
                 <a href='{passwordResetLink}'>Change password</a>
</div></div></div></body></html>";
                _ = new EmailHelper().SendEmail(User.User.Email, "Reset Password", Body, true);
            
            
            ViewBag.ResetUrl = passwordResetLink;
            return View("ForgotPassword");
        }
        [HttpPost]

        public async Task<IActionResult> Register(ApplicationUserVM model)
        {
            if (!ModelState.IsValid) return View(model);
            var Result = await applicationUserService.Create(model);
            if (Result.Status == true)
            {
                return await login(new LoginVM() { UserName = model.Email, Password = model.Password });
            }
            else
            {
                ModelState.AddModelError(nameof(model.Email), Result.Message);
                return View(model);
            }

        }
        [HttpPost]
        public async Task<IActionResult> login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var Result = await applicationUserService.Login(model);
                if (Result.Status)
                {

                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl)) return LocalRedirect(model.ReturnUrl);
                    else return RedirectToAction("index", "home");
                }
                ModelState.AddModelError("", Result.Message);
            }
            ViewBag.ReturnUrl = model.ReturnUrl;
            return View(model);
        }
        public async Task<IActionResult> LogOut()
        {
            await applicationUserService.LogOut();
            return Redirect("/home/index");
        }
        public IActionResult accessDenied()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }


    }
}
