using DummyPos.Models;

namespace DummyPos.Repositories.Interface
{
    public interface IBranchRepo
    {
        List<Branch> GetAllBranchList();
        Branch GetBranchById(int id); // To load data into the Edit form
        void UpdateBranch(Branch branch);
        void AddBranch(Branch branch);
        Branch GetBranchDetails(int id);
        void DeactivateBranch(int id);
        void ActivateBranch(int id);
    }
}
