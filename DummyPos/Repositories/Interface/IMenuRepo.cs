using DummyPos.Models;
using System.Collections.Generic;

namespace DummyPos.Repositories
{
    public interface IMenuRepo
    {
        List<ItemSearchViewModel> SearchItems(string keyword);
    }
}