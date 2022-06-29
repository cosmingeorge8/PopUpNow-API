using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PopUp_Now_API.Model;

namespace PopUp_Now_API.Interfaces
{
    public interface IImagesService
    {
        Task<Image> Upload(IFormFile formFile);
        Task<Image> Get(int imageId);
    }
}