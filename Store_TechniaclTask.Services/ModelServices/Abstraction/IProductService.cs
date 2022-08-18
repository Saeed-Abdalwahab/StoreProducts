using Store_TechniaclTask.DAL.Helper;
using Store_TechniaclTask.DAL.Model;
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

namespace Store_TechniaclTask.Services.ModelProducts.Abstraction
{

    public interface IProductService
    {
        
        Task<ProductVM> GetData(int ID);
        Task<IPagedList<Product>> GetData(CommonPageingRequest request, Func<IQueryable<Product>, IQueryable<Product>> func = null);
        IEnumerable<ProductVM> GetData(Expression<Func<Product, bool>> predicate = null);

        Task<CommonResponse<ProductVM>> Create(ProductVM VM);
        Task<CommonResponse<ProductVM>> Edit(ProductVM VM);
        Task<CommonResponse<ProductVM>> Remove(int ID);
        IEnumerable<ValidationResult> _ValidationResult(ProductVM vm);
    }

}
