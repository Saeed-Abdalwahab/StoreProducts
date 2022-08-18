using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Store_TechniaclTask.Services.HelperServices;
using Store_TechniaclTask.Services.ViewModels.DTO;

namespace Store_TechniaclTask.Services.CustomAttribute
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                //  MobileCommonResponse_PostData<object> apiResponse = new MobileCommonResponse_PostData<object>(modelErrors:  context.ModelState.GetModelStateErrors(),code:403)
                //;
                var apiResponse = CommonResponse<object>.GetResult(context.ModelState.GetModelStateErrors().FirstOrDefault().Value, context.ModelState.GetModelStateErrors());

                //foreach (var modelState in context.ModelState)
                //{
                //    modelState.Value.Errors.Select(a => a.ErrorMessage).ToList().FirstOrDefault()
                //    apiResponse.ModelErrors.Add(modelSta);
                //}

                //context.Result = new BadRequestObjectResult(context.ModelState);
                context.Result = new OkObjectResult(apiResponse);
            }
            //base.OnActionExecuting(context);
        }
    }
}
