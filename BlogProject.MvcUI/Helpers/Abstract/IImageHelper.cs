using BlogProject.Entities.DTOs.UserDto;
using BlogProject.Shared.Utilities.Results.Abstract;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BlogProject.MvcUI.Helpers.Abstract
{
    public interface IImageHelper
    {
        Task<IDataResult<UploadedImageDto>> UploadUserImage(string userName, IFormFile pictureFile, string folderName = "userImages");
        IDataResult<ImageDeletedDto> Delete(string pictureName);
    }
}
