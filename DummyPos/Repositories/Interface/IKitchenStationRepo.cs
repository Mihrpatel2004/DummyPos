using DummyPos.Models;
using System.Collections.Generic;

namespace DummyPos.Repositories.Interface
{
    public interface IKitchenStationRepo
    {
        List<KitchenStation> GetStationsByBranchId(int branchId);
        KitchenStation GetStationById(int id);
        void AddStation(KitchenStation station);
        void UpdateStation(KitchenStation station);
        void DeleteStation(int id);
        void DeactivateK_S(int id);
        void ActivateK_S(int id);
    }
}