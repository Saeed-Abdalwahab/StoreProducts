using Store_TechniaclTask.DAL.Enums;
using Store_TechniaclTask.DAL.Helper;
using Store_TechniaclTask.DAL.Model;
using Store_TechniaclTask.Services.ViewModels;
using Store_TechniaclTask.Services.ViewModels.Common;
using Store_TechniaclTask.Services.ViewModels.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Store_TechniaclTask.Services.ModelServices.Abstraction
{
    public interface IShoppingStoreService
    {

        Task<ShoppingStoreVM> GetData(int ID);
        IEnumerable<ShoppingStoreVM> GetUserData(ShoppingStoreStatus? shoppingStoreStatus=null);
        Task<IPagedList<ShoppingStore>> GetData(CommonPageingRequest request, Func<IQueryable<ShoppingStore>, IQueryable<ShoppingStore>> func = null);
        void SendRemindingMails(DateTime SendAt, string Email, string Url, int ShoppingStoreID);
        Task<CommonResponse<ShoppingStoreVM>> Create(ShoppingStoreVM VM);
        Task<CommonResponse<ShoppingStoreVM>> Edit(ShoppingStoreVM VM);
        Task<CommonResponse<ShoppingStoreVM>> Remove(int ID);
        IEnumerable<ValidationResult> _ValidationResult(ShoppingStoreVM vm);
    }

}
