using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Store_TechniaclTask.Services.HelperServices.Abstraction
{
    public interface IFilesServices
    {
        public string uploadFile(string PlaceOnServer, IFormFile file);
        Task<string> uploadFileAsync(string PlaceOnServer, IFormFile file);
        Task<List<string>> uploadFileAsync(string PlaceOnServer, List<IFormFile> files);
        Task<string> uploadFileFromUrl(string PlaceOnServer, string fileUrl);
        void DeleteFiles(string PlaceOnServer, string fileName);
        Task<string> uploadTempFilesAsync( MemoryStream memory, string extention = ".xlsx");
        void DeleteFiles(string PlaceOnServer, List<string> fileName);
    
    }
}
