using Microsoft.AspNetCore.Mvc;
using Store_TechniaclTask.Services.HelperServices;
using Store_TechniaclTask.Services.ModelProducts.Abstraction;
using Store_TechniaclTask.Services.Resources;
using Store_TechniaclTask.Services.ViewModels;
using Store_TechniaclTask.Services.ViewModels.Common;
using Store_TechniaclTask.Services.ViewModels.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _Store_TechniaclTask.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService ProductService;
        private readonly LocService locService;
        public ProductsController(IProductService ProductService,
            LocService locService
             )
        {
            this.ProductService = ProductService;
            this.locService = locService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetAll_Paged(CommonPageingRequest request)
        {
            var Results = await ProductService.GetData(request, (query) =>
             {

                 var Data = query.Where(x => request.Search == null || (x.Description.Contains(request.Search) || (x.Name.Contains(request.Search))));
                 Data.OrderByDescending(x => x.ID);
                 return Data;
             });
            return Ok(new
            {
                Date = Results,
                TotalCount = Results.TotalCount,
                HasPreviousPage = Results.HasPreviousPage,
                HasNextPage = Results.HasNextPage,
                TotalPages = Results.TotalCount
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductVM model)
        {
            if (!ModelState.IsValid) return Ok(CommonResponse<ProductVM>.GetResult(locService.GetLocalizedHtmlString("InvalidModelData"), ModelState.GetModelStateErrors()));
            var Result = await ProductService.Create(model);
            return Ok(Result);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductVM model)
        {
            if (!ModelState.IsValid) return Ok(CommonResponse<ProductVM>.GetResult(locService.GetLocalizedHtmlString("InvalidModelData"), ModelState.GetModelStateErrors()));
            var Result = await ProductService.Edit(model);
            return Ok(Result);
        }
        [HttpPost]
        public async Task<IActionResult> RemoveObj(int ID)
        {
            var Result = await ProductService.Remove(ID);
            return Ok(Result);
        }
        [HttpGet]
        public async Task<IActionResult> GetObj(int ID)
        {
            var Result = await ProductService.GetData(ID);
            return Ok(CommonResponse<ProductVM>.GetResult(Result));
        }
        public IActionResult GetAll()
        {
            var Result = ProductService.GetData();
            return Ok(new { data = Result });
        }
    }

}
