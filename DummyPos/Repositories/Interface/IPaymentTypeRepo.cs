using DummyPos.Models;
using System.Collections.Generic;

namespace DummyPos.Repositories.Interface
{
    public interface IPaymentTypeRepo
    {
        List<PaymentType> GetAllPaymentTypes();
        PaymentType GetPaymentTypeById(int id);
        void AddPaymentType(PaymentType paymentType);
        void UpdatePaymentType(PaymentType paymentType);
        void DeletePaymentType(int id);
        void ActivatePaymentType(int id);
        void DeactivatePaymentType(int id);
    }
}