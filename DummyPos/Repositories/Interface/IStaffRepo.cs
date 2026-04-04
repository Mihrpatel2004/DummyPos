using DummyPos.Models;

namespace DummyPos.Repositories.Interface
{
   
        public interface IStaffRepo
        {
            List<Staffs> GetAllStaff();
            Staffs GetStaffById(int id);

            // Notice we pass the plainPassword string here so the Repo can hash it
            void AddStaff(Staffs staff, string plainPassword);

            void UpdateStaff(Staffs staff);
            void DeactivateStaff(int id);


             List<Role> GetRoles();
            List<Branch> GetBranch();
        
    /* List<Staffs> GetAllStaff();

     Staffs GetStaffById(int id);

     public void AddStaff(Staffs staff, string plainPassword);

     public void UpdateStaff(Staffs staff);

     void DeactivateStaff(int id);

     List<Role> GetRoles();

     List<Branch> GetBranch();*/
}
}
