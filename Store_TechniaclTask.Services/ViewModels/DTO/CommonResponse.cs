using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Store_TechniaclTask.DAL.Helper;
using Store_TechniaclTask.Services.HelperServices;

namespace Store_TechniaclTask.Services.ViewModels.DTO
{

    public class CommonResponse<T>
    {
        public int Code { get; set; } = 200;
        public bool Status { get; set; }
        public string Message { get; set; }
        public List<string> ListOfMessages { get; set; }
        public T Data { get; set; }
        public Dictionary<string, string> ModelErrors { get; set; }
        public static CommonResponse<T> GetResult(int code, bool Status, string msg, T data = default(T), Dictionary<string, string> ModelErrors = default(Dictionary<string, string>))
        {
            return new CommonResponse<T>
            {
                Code = code,
                Message = msg,
                Data = data,
                ModelErrors = ModelErrors,
                Status = Status

            };
        }
        public static CommonResponse<T> GetResult(string msg, Dictionary<string, string> ModelErrors, T data = default(T))
        {
            return new CommonResponse<T>
            {
                Code = 403,
                Message = msg,
                Data = data,
                ModelErrors = ModelErrors,
                Status = false

            };
        }
        public static CommonResponse<T> GetResult(bool Status, List<string> msgs, T data = default(T))
        {
            return new CommonResponse<T>
            {
                ListOfMessages = msgs,
                Data = data,
                Status = Status

            };
        }
        public static CommonResponse<T> GetResult(bool Status, string msg, T data = default(T))
        {
            return new CommonResponse<T>
            {
                Message = msg,
                Data = data,
                Status = Status,
                Code=Status==true?200:403
                
            };
        }
        public static CommonResponse<T> GetResult(T data, int statusCode = 200)
        {
            return new CommonResponse<T>
            {
                Data = data,
                Code = statusCode

            };
        }
    }
    public class MobileCommonResponse_PostData<T>:MobileCommonResponse_GetData<T>
    {
        public MobileCommonResponse_PostData(Dictionary<string, string> modelErrors=null,string message = "", int code = 200, T data = default(T)):base(message,code,data)
        {
            ModelErrors = modelErrors ?? new Dictionary<string, string>();
            if(string.IsNullOrEmpty(message) && modelErrors != null)
            {
                var fristmessege = ExtentionMethods.GetDisplayName(modelErrors.FirstOrDefault().Key ?? "") + " : " + modelErrors.FirstOrDefault().Value;
                Message = fristmessege;
            }
            else
            {
                Message = message;
            }
        }
        public Dictionary<string, string> ModelErrors {private get; set; } 
    }

    public class MobileCommonResponse_GetData<T>
    {
        public T Data { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public MobileCommonResponse_GetData(string message="",int code=200,T data = default(T))
        {
            Message = message;
            Code = code;
            Data = data;
        } 


    }
    //public class PagedDataResponse<T> :IPagedList<T>  
    //{

    //    public PagedDataResponse(T data , string message="",int code=200)
    //    {
    //        Message = message;
    //        Code = code;
    //        Data = data;

    //    } 
    //}
}
