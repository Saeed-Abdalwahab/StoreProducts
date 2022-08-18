using AutoMapper;
using Microsoft.AspNetCore.Http;
using Repository.Abstraction;
using Store_TechniaclTask.DAL.Enums;
using Store_TechniaclTask.DAL.Helper;
using Store_TechniaclTask.DAL.Model;
using Store_TechniaclTask.Services.HelperServices;
using Store_TechniaclTask.Services.HelperServices.Abstraction;
using Store_TechniaclTask.Services.ModelProducts.Abstraction;
using Store_TechniaclTask.Services.ModelServices.Abstraction;
using Store_TechniaclTask.Services.Resources;
using Store_TechniaclTask.Services.ViewModels;
using Store_TechniaclTask.Services.ViewModels.Common;
using Store_TechniaclTask.Services.ViewModels.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Store_TechniaclTask.Services.ModelServices.Implementation
{
    public class ShoppingStoreService : IShoppingStoreService
    {
        private readonly IMapper mapper;
        private readonly IRepository<ShoppingStore> repository;
        private readonly IRepository<ShoppingStoreDetails> shoppingStoreDetails_repository;
        private readonly IRepository<ShoppingStoreReminder> ShoppingStoreReminder_repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IProductService productService;
        private readonly LocService locService;
        private readonly IGloableMethodsService gloableMethodsService;

        public ShoppingStoreService(IMapper mapper,
            IRepository<ShoppingStore> repository,
                              IUnitOfWork unitOfWork,
                              LocService locService,
                              IGloableMethodsService gloableMethodsService
, IProductService productService, IRepository<ShoppingStoreDetails> shoppingStoreDetails_repository, IRepository<ShoppingStoreReminder> shoppingStoreReminder_repository)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.locService = locService;
            this.gloableMethodsService = gloableMethodsService;
            this.productService = productService;
            this.shoppingStoreDetails_repository = shoppingStoreDetails_repository;
            ShoppingStoreReminder_repository = shoppingStoreReminder_repository;
        }
        public async Task<CommonResponse<ShoppingStoreVM>> Create(ShoppingStoreVM VM)
        {
            bool Status = true;
            string Message = "SavedSuccessfully";
            try
            {
                VM.UserID = gloableMethodsService.CurrentUserID();
                var listOfProducts = productService.GetData(x => VM.DetailsVM.Select(xx => xx.ProductID).Contains(x.ID));
                // for make shur every product with valid price
                VM.DetailsVM.ForEach(item => item.ProductPrice = listOfProducts.First(x => x.ID == item.ProductID).Price);
                VM.TotalPrices = VM.DetailsVM.Sum(x => x.ProductPrice);
                var Obj = mapper.Map(VM, new ShoppingStore());
                await repository.AddAsync(Obj);
                await unitOfWork.SaveChangesAsync();
                VM.ID = Obj.ID;
            }
            catch
            {
                Status = false;
                Message = "Err";
            }
            return CommonResponse<ShoppingStoreVM>.GetResult(Status, locService.GetLocalizedHtmlString(Message), VM);
        }

        public async Task<CommonResponse<ShoppingStoreVM>> Edit(ShoppingStoreVM VM)
        {
            bool Status = true;
            string Message = "SavedSuccessfully";
            var CurrentUserID = gloableMethodsService.CurrentUserID();
            var obj = await repository.GetAsync(VM.ID);
            if (obj == null || CurrentUserID != obj.UserID)
            {
                return CommonResponse<ShoppingStoreVM>.GetResult(false, locService.GetLocalizedHtmlString("NoAccessForThis"));
            }
            if (obj.ShoppingStoreStatus == ShoppingStoreStatus.Saved)
            {
                return CommonResponse<ShoppingStoreVM>.GetResult(false, locService.GetLocalizedHtmlString("CanNotEditSaved"));
            }
            using (var transation = await unitOfWork.CreatTransactionAsync())
            {
                try
                {
                    shoppingStoreDetails_repository.RemoveRange(obj.Details.Where(x => !VM.DetailsVM.Any(xx => xx.ID == x.ID)));
                    await unitOfWork.SaveChangesAsync();
                    repository.Detached(obj);
                    mapper.Map(VM, obj);
                    obj.UserID = CurrentUserID;
                    repository.Update(obj);
                    await unitOfWork.SaveChangesAsync();
                    await unitOfWork.CommitAsync(transation);
                }
                catch
                {
                    Status = false;
                    Message = "Err";
                    await unitOfWork.RollBackAsync(transation);
                    await transation.DisposeAsync();
                }
            }




            return CommonResponse<ShoppingStoreVM>.GetResult(Status, locService.GetLocalizedHtmlString(Message), VM);
        }

        public IEnumerable<ShoppingStoreVM> GetUserData(ShoppingStoreStatus? shoppingStoreStatus = null)
        {
            var CurrentUserID = gloableMethodsService.CurrentUserID();
            var obj = repository.GetAll(x => x.UserID == CurrentUserID && x.ShoppingStoreStatus == shoppingStoreStatus).OrderByDescending(x=>x.RegistrationDate);
            return mapper.Map(obj, new List<ShoppingStoreVM>());
        }
        public virtual async Task<IPagedList<ShoppingStore>> GetData(CommonPageingRequest request, Func<IQueryable<ShoppingStore>, IQueryable<ShoppingStore>> func = null)
        {
            var PagedData = await repository.GetAllPagedAsync(func, request.pageNumber - 1, request.pageSize, false, true);
            var tt = PagedData.MetaData();
            return PagedData;
        }

        public async Task<ShoppingStoreVM> GetData(int ID)
        {
            var obj = await repository.GetAsync(ID);
            if (obj == null || obj.UserID != gloableMethodsService.CurrentUserID()) return null;
            return mapper.Map(obj, new ShoppingStoreVM());
        }

        public async Task<CommonResponse<ShoppingStoreVM>> Remove(int ID)
        {

            string Message = locService.GetLocalizedHtmlString("RemovedSuccessfully");
            bool Status = true;
            var Obj = repository.Get(ID);
            var CurrentUserID = gloableMethodsService.CurrentUserID();
            if (CurrentUserID != Obj.UserID)
            {
                return CommonResponse<ShoppingStoreVM>.GetResult(false, locService.GetLocalizedHtmlString("NoAccessForThis"));
            }
            using (var transation = await unitOfWork.CreatTransactionAsync())
            {
                try
                {
                    repository.Remove(Obj);
                    await unitOfWork.SaveChangesAsync();
                    await unitOfWork.CommitAsync(transation);
                }
                catch
                {
                    Status = false;
                    await unitOfWork.RollBackAsync(transation);
                    await transation.DisposeAsync();
                }
            }


            return Status ? CommonResponse<ShoppingStoreVM>.GetResult(Status, Message) :
                CommonResponse<ShoppingStoreVM>.GetResult(Status, repository.GetDependenciesNames(Obj).ToList());
        }
        public IEnumerable<ValidationResult> _ValidationResult(ShoppingStoreVM vm)
        {
            yield return ValidationResult.Success;


        }
      public  void SendRemindingMails(DateTime SendAt,string Email,string Url,int ShoppingStoreID)
        {
            //var link = Url.Action("index", "Home", new { ID= ShoppingStoreID}, Request.Scheme);
            var Body = $@"<!DOCTYPE html><html lang='en'><head><meta charset='UTF-8' />
                 <meta http-equiv='X-UA-Compatible' content='IE=edge' />
                 <meta name='viewport' content='width=device-width, initial-scale=1.0' /> 
                 <title>Remind Checked Items</title></head><body style='margin: 3px'><div class='container' style='text-align: center; background-color: #fbf4e4; height: 110vh'><div class='body-email'
style='position: absolute;top: 50%; left: 50%; transform: translate(-40%, -40%);'>
                 <div class='title'><h2 style='margin:
3px'>Friendzr</h2></div><h1>You Ask To Reminder For Items You Checked</h1><div
style='font-size: 17px'>  We got a request to Remind you
for your  shopping check list  .</div><div class='code'style='font-size:
20px; font-weight: bold; margin: 17px'></br> 
                 <a href='{Url}'>go to list</a>
</div></div></div></body></html>";
            System.Threading.Timer timer;
            DateTime current = DateTime.Now;
            TimeSpan timeToGo =  (SendAt - current);
            TimeSpan Delay = SendAt - DateTime.Now;
            if (timeToGo > TimeSpan.Zero)
            {
                //time not passed
            timer = new System.Threading.Timer(x =>
            {
                new EmailHelper().SendEmail(Email, "Reminder", Body, true);
                 
            }, null, timeToGo, Timeout.InfiniteTimeSpan);
            //}, null, timeToGo, timeToGo);
            }

            
        }
    
    
    }

}
