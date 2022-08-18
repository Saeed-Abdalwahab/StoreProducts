using Store_TechniaclTask.Services.HelperServices.Abstraction;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Store_TechniaclTask.Services.HelperServices.Implementation
{
    public class FilesServices : IFilesServices
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public FilesServices(IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.httpContextAccessor = httpContextAccessor;
        }
        public void DeleteFiles(string PlaceOnServer, string fileName)
        {
            string DeletetedfilePathpath = hostingEnvironment.WebRootPath + PlaceOnServer + fileName;
            if (File.Exists(DeletetedfilePathpath)) File.Delete(DeletetedfilePathpath);
        }
        public void DeleteFiles(string PlaceOnServer, List<string> fileName)
        {
            foreach (var item in fileName)
            {
                string DeletetedfilePathpath = hostingEnvironment.WebRootPath + PlaceOnServer + item;
                if (File.Exists(DeletetedfilePathpath)) File.Delete(DeletetedfilePathpath);
            }
        }
        public async Task<string> uploadFileFromUrl(string PlaceOnServer, string fileUrl)
        {

            if (fileUrl == null || fileUrl == "") return null;
            string filename = System.IO.Path.GetFileName(fileUrl);


            var Path = fileUrl;
            using (HttpClient client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Add("authorization", access_token); //if any
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(fileUrl);

                if (response.IsSuccessStatusCode)
                {

                    HttpContent content = response.Content;


                    var contentStream = await content.ReadAsStreamAsync(); // get the actual content stream



                    string ImagePath = hostingEnvironment.WebRootPath + PlaceOnServer + filename;

                    using (var FileStream = new FileStream(ImagePath, FileMode.Create))
                    {

                        await contentStream.CopyToAsync(FileStream);
                    }
                }
                else
                {
                    filename = null;
                }
            }
            return filename;


        }
        bool checkExtention(IFormFile file)
        {
            var ValidExtentions = new string[] { ".xlsx",".ico", ".xls", ".jpg", ".wav", ".flv", ".mp4", ".ppt", ".jpeg", ".png", ".gif",".jfif", ".docx", ".docxs", ".pdf", ".word" ,".docx",
                ".mp3",".ogg","m4a",".mp4",".3gp",".webm",".flv",".avi",".jpg",".gif",".jpeg",".png",".doc",".xls",".ppt",".pptx",".rar",".zip",".pdf",".txt",".text"};
            var extention = Path.GetExtension(file.FileName.ToLower()).ToString();
            if (ValidExtentions.Contains(extention))
            {
                return true;
            }
            return false;
        }
        public string uploadFile(string PlaceOnServer, IFormFile file)
        {


            if (file == null) return null;
            //check for extention validation
            if (!checkExtention(file)) return null;
            //Check If Image AleradyExist Befor In Server 
            else if (File.Exists(hostingEnvironment.WebRootPath + PlaceOnServer + file.FileName))
            {
                return file.FileName;
            }
            else
            {
                var PlaceOnServerUrl = hostingEnvironment.WebRootPath + PlaceOnServer;
                if (!File.Exists(PlaceOnServerUrl))
                {
                    Directory.CreateDirectory(PlaceOnServerUrl);
                }
                string extention = Path.GetExtension(file.FileName);

                string UniqImageName = Guid.NewGuid().ToString() + "_" + Regex.Replace(file.FileName, @"[^0-9a-zA-Z_]+", "") + extention;
                if (UniqImageName.Length > 100)
                {
                    UniqImageName = Guid.NewGuid().ToString() + "_NewName" + extention;
                }

                string ImagePath = hostingEnvironment.WebRootPath + PlaceOnServer + UniqImageName;

                using (var FileStream = new FileStream(ImagePath, FileMode.Create))
                {
                    file.CopyTo(FileStream);
                }
                return UniqImageName;
            }
        }


        public async Task<List<string>> uploadFileAsync(string PlaceOnServer, List<IFormFile> files)
        {
            List<string> Names = new List<string>();
           
            if(files==null) return Names;
            foreach (var file in files)
            {
                var name = await uploadFileAsync(PlaceOnServer, file);
                if (!string.IsNullOrEmpty(name))
                    Names.Add(name);
            }
            return Names;
        }
        public async Task<string> uploadFileAsync(string PlaceOnServer, IFormFile file)
        {
            if (file == null) return null;

            //check for extention validation
            if (!checkExtention(file)) throw new Exception("NotAcceptedFileExtentionPleaseContactWithAdmin");
            //Check If Image AleradyExist Befor In Server 
            else if (File.Exists(hostingEnvironment.WebRootPath + PlaceOnServer + file.FileName))
            {

                return file.FileName;
            }
            else
            {
                var PlaceOnServerUrl = hostingEnvironment.WebRootPath + PlaceOnServer;
                if (!Directory.Exists(PlaceOnServerUrl))
                {
                    Directory.CreateDirectory(PlaceOnServerUrl);
                }
                string extention = Path.GetExtension(file.FileName);

                string UniqImageName = Guid.NewGuid().ToString() + "_" + Regex.Replace(file.FileName, @"[^0-9a-zA-Z_]+", "") + extention;
                if (UniqImageName.Length > 100)
                {
                    UniqImageName = Guid.NewGuid().ToString() + "_NewName" + extention;
                }

                string ImagePath = hostingEnvironment.WebRootPath + PlaceOnServer + UniqImageName;

                using (var FileStream = new FileStream(ImagePath, FileMode.Create))
                {
                    await file.CopyToAsync(FileStream);
                }
                return UniqImageName;

            }
        }
        public async Task<string> uploadTempFilesAsync(MemoryStream memory, string extention = ".xlsx")
        {
            string PlaceOnServer = "/TempFiles/";
            string UniqImageName = Guid.NewGuid().ToString() + extention;

            var PlaceOnServerUrl = hostingEnvironment.WebRootPath + PlaceOnServer;
            if (!Directory.Exists(PlaceOnServerUrl))
            {
                Directory.CreateDirectory(PlaceOnServerUrl);
            }
            _ = DeleteTempFiles(PlaceOnServerUrl);
            using (FileStream file = new FileStream(PlaceOnServerUrl + UniqImageName, FileMode.Create, System.IO.FileAccess.Write))
            {
                await memory.CopyToAsync(file);
            }


            return PlaceOnServer + UniqImageName;
        }

        Task DeleteTempFiles(string fullPath)
        {
            return Task.Run(() =>
             {
                 var files = Directory.GetFiles(fullPath);
                 foreach (var item in files)
                 {
                     var lastModified = System.IO.File.GetLastWriteTimeUtc(item);
                     if ((DateTime.UtcNow - lastModified).Days > 1)
                     {
                         File.Delete(item);
                     }


                 }
             });
        }
    }
}
