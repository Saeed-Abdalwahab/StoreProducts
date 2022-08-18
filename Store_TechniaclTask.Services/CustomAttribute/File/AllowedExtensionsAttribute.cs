using Store_TechniaclTask.Services.ViewModels.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Store_TechniaclTask.DAL.Enums;
using Microsoft.AspNetCore.Http;

namespace Services.CustomAttribute.File
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {

        private readonly string[] _extensions;
        public AllowedExtensionsAttribute(AttachmentType attachmentType)
        {
            if (attachmentType == AttachmentType.Image)
                _extensions = AttachmentOnserver.ImageExtentions;
            else if (attachmentType == AttachmentType.Video)
                _extensions = AttachmentOnserver.VideoExtentions;
            else if (attachmentType == AttachmentType.File)
                _extensions = AttachmentOnserver.FileExtentions;
            else
                _extensions = AttachmentOnserver.AllowedExtentions();

        }
        public AllowedExtensionsAttribute()
        {
            _extensions = AttachmentOnserver.AllowedExtentions();
        } 
        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }
        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
            else
            {
                var files = (value as List<IFormFile>) ?? new List<IFormFile>() ;
                foreach (var item in files)
                {
                    var extension = Path.GetExtension(item.FileName);
                    if (!_extensions.Contains(extension.ToLower()))
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }
                
            }
            return ValidationResult.Success;
        }
        public string GetErrorMessage()
        {
            return string.Format(ErrorMessage, _extensions.Aggregate((x,y)=>x+","+y));
        }
    }
}
