using BlogProject.Entities.ComplexTypes;
using BlogProject.Entities.DTOs.UserDto;
using BlogProject.Shared.Utilities.Results.Abstract;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BlogProject.MvcUI.Helpers.Abstract
{
    public interface IImageHelper
    {
        Task<IDataResult<UploadedImageDto>> Upload(string name, IFormFile pictureFile, PictureType pictureType, string folderName = null);
        IDataResult<ImageDeletedDto> Delete(string pictureName);
    }
}
