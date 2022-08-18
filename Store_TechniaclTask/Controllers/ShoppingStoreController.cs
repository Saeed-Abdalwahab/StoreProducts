using Microsoft.AspNetCore.Mvc;
using Store_TechniaclTask.DAL.Enums;
using Store_TechniaclTask.Services.CustomAttribute;
using Store_TechniaclTask.Services.HelperServices;
using Store_TechniaclTask.Services.HelperServices.Abstraction;
using Store_TechniaclTask.Services.ModelServices.Abstraction;
using Store_TechniaclTask.Services.Resources;
using Store_TechniaclTask.Services.ViewModels;
using Store_TechniaclTask.Services.ViewModels.Common;
using Store_TechniaclTask.Services.ViewModels.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace _Store_TechniaclTask.Web.Controllers
{
    public class ShoppingStoreController : Controller
    {
        private readonly IShoppingStoreService ShoppingStoreService;
        private readonly LocService locService;
        private readonly IGloableMethodsService gloableMethodsService;
        public ShoppingStoreController(IShoppingStoreService ShoppingStoreService,
            LocService locService
, IGloableMethodsService gloableMethodsService)
        {
            this.ShoppingStoreService = ShoppingStoreService;
            this.locService = locService;
            this.gloableMethodsService = gloableMethodsService;
        }
        public IActionResult Saved()
        {
            return View();
        }
        public IActionResult Archived()
        {
            return View();
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Save(ShoppingStoreVM model)
        {
            model.ShoppingStoreStatus = ShoppingStoreStatus.Saved;
            CommonResponse<ShoppingStoreVM> Result;
            if (model?.ID > 0)
            {
                Result = await ShoppingStoreService.Edit(model);

            }
            else
            {
                Result = await ShoppingStoreService.Create(model);
            }
            if (Result.Status == true && model.ReminderTime != null)
            {
                var link = Url.Action("index", "Home", new { ID = Result.Data?.ID }, Request.Scheme);
                ShoppingStoreService.SendRemindingMails(model.ReminderTime.Value, gloableMethodsService.CurrentUserEmail(), link, Result.Data?.ID ?? 0);
            }
            return Ok(Result);

        }
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Archive(ShoppingStoreVM model)
        {
            model.ShoppingStoreStatus = ShoppingStoreStatus.Archived;
            CommonResponse<ShoppingStoreVM> Result;
            if (model?.ID > 0)
            {
                Result = await ShoppingStoreService.Edit(model);

            }
            else
            {
                Result = await ShoppingStoreService.Create(model);
            }
            if (Result.Status == true && model.ReminderTime != null)
            {
                var link = Url.Action("index", "Home", new { ID = Result.Data?.ID }, Request.Scheme);
                ShoppingStoreService.SendRemindingMails(model.ReminderTime.Value, gloableMethodsService.CurrentUserEmail(), link,  Result.Data?.ID??0);
            }
            return Ok(Result);
        }
       
        [HttpPost]
        public async Task<IActionResult> RemoveObj(int ID)
        {
            var Result = await ShoppingStoreService.Remove(ID);
            return Ok(Result);
        }

        [HttpGet]
        public async Task<IActionResult> GetObj(int ID)
        {
            var Result = await ShoppingStoreService.GetData(ID);
            return Ok(CommonResponse<ShoppingStoreVM>.GetResult(Result));
        }
        public IActionResult GetAll()
        {
            var Result = ShoppingStoreService.GetUserData();
            return Ok(new { data = Result });
        }
        public IActionResult UserSavedProducts()
        {
            var Result = ShoppingStoreService.GetUserData(ShoppingStoreStatus.Saved);
            return Ok(new { data = Result });
        }
        public IActionResult UserArchivedProducts()
        {
            var Result = ShoppingStoreService.GetUserData(ShoppingStoreStatus.Archived);
            return Ok(new { data = Result });
        }
    }

}
