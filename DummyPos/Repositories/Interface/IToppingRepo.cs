using DummyPos.Models;

namespace DummyPos.Repositories.Interface
{
    public interface IToppingRepo
    {
        List<Toppings> GetAllToppings();
        Toppings GetToppingById(int id);
        void AddTopping(Toppings topping);
        void UpdateTopping(Toppings topping);
        void DeleteTopping(int id);
        void ActivateTopping(int id);
        void DeactivateTopping(int id);
    }
}
