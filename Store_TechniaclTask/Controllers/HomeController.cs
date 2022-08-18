using Store_TechniaclTask.Services.Enums;
using Store_TechniaclTask.Services.HelperServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Store_TechniaclTask.Services.ViewModels.DTO;
using Store_TechniaclTask.Services.ModelServices.Abstraction;
using Store_TechniaclTask.Services.ViewModels;
using Store_TechniaclTask.DAL.Enums;

namespace Store_TechniaclTask.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IShoppingStoreService shoppingStoreService;

        public HomeController(ILogger<HomeController> logger, IShoppingStoreService shoppingStoreService)
        {
            _logger = logger;
            this.shoppingStoreService = shoppingStoreService;
        }
        public IActionResult Index(int ID)
        {
            var obj = shoppingStoreService.GetData(ID).Result;
            var model =(obj==null||obj.ShoppingStoreStatus!=ShoppingStoreStatus.Archived)? new ShoppingStoreVM():obj;
          
            return View(model);
        }
        [AllowAnonymous]

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
   
        [AllowAnonymous]
        [HttpPost]
        public IActionResult SetLanguage(Language language)
        {
            CreateUserCookie_Languages(language);
            return Ok();
        }
        private void CreateUserCookie_Languages(Language language)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Append("Usre_Culture", language == Language.Arabic ? "ar-EG" : "en-US", option);

        }
    }
}
