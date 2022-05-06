using BlogProject.Entities.DTOs.UserDto;
using BlogProject.MvcUI.Helpers.Abstract;
using BlogProject.Shared.Utilities.Extensions;
using BlogProject.Shared.Utilities.Results.Abstract;
using BlogProject.Shared.Utilities.Results.ComplexTypes;
using BlogProject.Shared.Utilities.Results.Concrete;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BlogProject.MvcUI.Helpers.Concrete
{
    public class ImageHelper : IImageHelper
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _wwwroot;
        private readonly string imgFolder = "img";

        public ImageHelper(IWebHostEnvironment env)
        {
            _env = env;
            _wwwroot = _env.WebRootPath;
        }

        public IDataResult<ImageDeletedDto> Delete(string pictureName)
        {
            var fileToDelete = Path.Combine($"{_wwwroot}/{imgFolder}", pictureName);
            if (File.Exists(fileToDelete))
            {
                var fileInfo = new FileInfo(fileToDelete);
                var imageDeleteDto = new ImageDeletedDto
                {
                    FullName = pictureName,
                    Extensions = fileInfo.Extension,
                    Path = fileInfo.FullName,
                    Size = fileInfo.Length
                };
                File.Delete(fileToDelete);
                return new DataResult<ImageDeletedDto>(ResultStatus.Success,imageDeleteDto);
            }
            else
            {
                return new DataResult<ImageDeletedDto>(ResultStatus.Error, "Dosya bulunamadı",null);
            }
        }

        public async Task<IDataResult<UploadedImageDto>> UploadUserImage(string userName, IFormFile pictureFile, string folderName = "userImages")
        {
            //Klasör kontrolü yapıyoruz dosya yolu mevcut değilse yeni klasör oluşturuyor.
            if (!Directory.Exists($"{_wwwroot}/{imgFolder}/{folderName}"))
            {
                Directory.CreateDirectory($"{_wwwroot}/{imgFolder}/{folderName}");
            }


            string oldFileName = Path.GetFileNameWithoutExtension(pictureFile.FileName);
            string fileExtension = Path.GetExtension(pictureFile.FileName);

            DateTime dateTime = DateTime.Now;

            string newFileName = $"{userName}_{dateTime.FullDateAndTimeStringWithUnderscore()}{fileExtension}";

            var path = Path.Combine($"{_wwwroot}/{imgFolder}/{folderName}", newFileName);

            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await pictureFile.CopyToAsync(stream);
            }

            return new DataResult<UploadedImageDto>(ResultStatus.Success,"Resim Eklendi", new UploadedImageDto
            {
                FullName = $"{folderName}/{newFileName}",
                OldName = oldFileName,
                Extension = fileExtension,
                FolderName = folderName,
                Path = path,
                Size = pictureFile.Length
                
            });
           
        }
    }
}
