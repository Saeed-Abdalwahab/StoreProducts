using System;
using System.Collections.Generic;
using System.Text;

namespace Store_TechniaclTask.Services.ViewModels.DTO
{
  public   class AttachmentOnserver
    {
        public static string ClinicAttachment{ get { return "/Attachements/ClinicAttachment/"; } }
        public static string UsersAttachment { get { return "/Attachements/UsersAttachment/"; } }
        public static string OffersAttachment { get { return "/Attachements/OffersAttachment/"; } }
        public static string DevicesAttachment { get { return "/Attachements/DevicesAttachment/"; } }
        public static string ServiceDeviceCases { get { return "/Attachements/ServiceDeviceCases/"; } }
        public static string DevicesCasesAttachment { get { return "/Attachements/DevicesAttachment/DevicesCases/"; } }
        public static string DepartmentsAttachment { get { return "/Attachements/DepartmentsAttachment/"; } }
        public static string ApplicationLogo { get { return "/Attachements/Logos/"; } }
        public static string[] AllowedExtentions()
        {
            var image = ImageExtentions;
            var file = FileExtentions;
            var video = VideoExtentions;
            var Other = OtherExtentions;
            var list = new List<string>();
            list.AddRange(image);
            list.AddRange(file);
            list.AddRange(video);
            list.AddRange(Other);
            return list.ToArray();
            //return new string[] { ".jpg", ".jpeg", ".png", ".docx", ".doc", ".pdf", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pps", ".ppsx", /*".mid", ".midi", ".rm", ".ram", ".wma", ".aac", ".wav", ".ogg",*/ ".mp3", ".mp4", ".mpg", ".mpeg", ".avi", ".wmv", ".mov", ".swf", ".flv", ".webm" };
        }
        public static readonly string[] ImageExtentions= new string[] { ".jpg", ".jpeg", ".png" , ".bmp" };
        public static readonly string[] VideoExtentions = new string[] { ".mp3", ".mp4", ".mpg", ".mpeg", ".avi", ".wmv", ".mov", ".swf", ".flv", ".webm" };
        public static readonly string[] FileExtentions = new string[] { ".docx", ".doc", ".pdf", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pps", ".ppsx" };
        public static readonly string[] OtherExtentions = new string[] {/* ".docx", ".doc", ".pdf", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pps", ".ppsx"*/ };

     
    


    }
}
