using DummyPos.Models;

namespace DummyPos.Repositories.Interface
{
    public interface IRoleRepo
    {
        List<Role> GetAllRole();

        void DeactivateRole(int id);
        void ActivateRole(int id);
        public void DeleteRole(int id);
        Role GetRoleById(int id);

        public void AddRole(Role role);

        public void UpdateRole(Role role);

    
       

    }
}
