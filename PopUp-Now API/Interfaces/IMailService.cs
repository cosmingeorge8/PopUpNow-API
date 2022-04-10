using System.Threading.Tasks;
using PopUp_Now_API.Model;

namespace PopUp_Now_API.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(Email email);
    }
}