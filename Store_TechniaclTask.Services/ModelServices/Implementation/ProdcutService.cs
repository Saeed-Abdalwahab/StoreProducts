using AutoMapper;
using Microsoft.AspNetCore.Http;
using Repository.Abstraction;
using Store_TechniaclTask.DAL.Helper;
using Store_TechniaclTask.DAL.Model;
using Store_TechniaclTask.Services.ModelProducts.Abstraction;
using Store_TechniaclTask.Services.Resources;
using Store_TechniaclTask.Services.ViewModels;
using Store_TechniaclTask.Services.ViewModels.Common;
using Store_TechniaclTask.Services.ViewModels.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store_TechniaclTask.Services.ModelProducts.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Product> repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly LocService locService;

        public ProductService(IMapper mapper, IRepository<Product> repository,
                              IUnitOfWork unitOfWork, LocService locService
                              )
        {
            this.mapper = mapper;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.locService = locService;

        }
        public async Task<CommonResponse<ProductVM>> Create(ProductVM VM)
        {

            bool Status = true;
            string Message = "SavedSuccessfully";
            try
            {
                var Obj = mapper.Map(VM, new Product());
                await repository.AddAsync(Obj);
                await unitOfWork.SaveChangesAsync();
                VM.ID = Obj.ID;
            }
            catch 
            {
                Status = false;
                Message = "Err";
            }

            return CommonResponse<ProductVM>.GetResult(Status, locService.GetLocalizedHtmlString(Message), VM);
        }

        public async Task<CommonResponse<ProductVM>> Edit(ProductVM VM)
        {
            bool Status = true;
            string Message = "SavedSuccessfully";

            var obj = await repository.GetAsync(VM.ID);
            
                using (var transation = await unitOfWork.CreatTransactionAsync())
                {
                    try
                    {

                        mapper.Map(VM, obj);
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

            
          

            return CommonResponse<ProductVM>.GetResult(Status, locService.GetLocalizedHtmlString(Message), VM);
        }

        public IEnumerable<ProductVM> GetData(Expression<Func<Product, bool>> predicate = null)
        {
            var obj = predicate == null? repository.GetAll():repository.GetAll(predicate);
            return mapper.Map(obj, new List<ProductVM>());
        }
        public virtual async Task<IPagedList<Product>> GetData(CommonPageingRequest request, Func<IQueryable<Product>, IQueryable<Product>> func = null)
        {
            var PagedData = await repository.GetAllPagedAsync(func, request.pageNumber-1, request.pageSize, false, true);
            var tt = PagedData.MetaData();
            return PagedData;
        }

        public async Task<ProductVM> GetData(int ID)
        {
            var obj = await repository.GetAsync(ID);
            if (obj == null) return null;
            return mapper.Map(obj, new ProductVM());
        }

        public async Task<CommonResponse<ProductVM>> Remove(int ID)
        {
            string Message = locService.GetLocalizedHtmlString("RemovedSuccessfully");
            bool Status = true;
            var Obj = repository.Get(ID);
           
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

            
            return Status ? CommonResponse<ProductVM>.GetResult(Status, Message) :
                CommonResponse<ProductVM>.GetResult(Status, repository.GetDependenciesNames(Obj).ToList());
        }
        public IEnumerable<ValidationResult> _ValidationResult(ProductVM vm)
        {
            if (repository.Any(x => x.ID != vm.ID && x.Name.ToLower() == vm.Name.ToLower()))
            {
                var Message = string.Format(locService.GetLocalizedHtmlString("AlreadyExist"), vm.Name);
                yield return new ValidationResult(Message, new[] { nameof(vm.Name) });
            }
         

        }
       
    }

}
