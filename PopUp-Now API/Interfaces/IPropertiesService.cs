using System.Collections.Generic;
using System.Threading.Tasks;
using PopUp_Now_API.Model;
using PopUp_Now_API.Model.Requests;

namespace PopUp_Now_API.Interfaces
{
    public interface IPropertiesService
    {
        Task<List<Property>> GetAll(string query);
        Task<List<Property>> GetAll();
        Task<Property> Get(int propertyId);
        Task<Property> Delete(int propertyId);
        Task<Property> Add(PropertyRequest propertyRequest, string user);
        Task<bool> Update(PropertyRequest propertyRequest);
        Task<List<Property>> GetByCategory(int categoryId);
        Task<List<Property>> GetByLandlord(string email);
    }
}