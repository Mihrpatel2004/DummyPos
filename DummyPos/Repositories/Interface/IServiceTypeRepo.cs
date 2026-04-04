using DummyPos.Models;

namespace DummyPos.Repositories.Interface
{
    public interface IServiceTypeRepo
    {
        List<ServiceType> GetAllServiceTypes();
        ServiceType GetServiceTypeById(int id);
        void AddServiceType(ServiceType service);
        void UpdateServiceType(ServiceType service);
        void ActivateServiceType(int id);
        void DeactivateServiceType(int id);
    }
}
