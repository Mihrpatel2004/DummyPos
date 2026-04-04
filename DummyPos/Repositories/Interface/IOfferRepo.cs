using DummyPos.Models;
using System.Collections.Generic;

namespace DummyPos.Repositories.Interface
{
    public interface IOfferRepo
    {
        List<Offer> GetAllOffers();
        Offer GetOfferById(int id);
        void AddOffer(Offer offer);
        void UpdateOffer(Offer offer);
        void DeleteOffer(int id);
        void ActivateOffer(int id);
        void DeactivateOffer(int id);
    }
}