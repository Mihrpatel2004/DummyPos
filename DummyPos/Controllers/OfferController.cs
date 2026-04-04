using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DummyPos.Controllers
{
    public class OfferController : Controller
    {
        private readonly IOfferRepo _repo;

        public OfferController(IOfferRepo repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            var list = _repo.GetAllOffers();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Set defaults: Starts today, Ends in 7 days
            return View(new Offer
            {
                Is_Active = true,
                Start_Date = DateTime.Today,
                End_Date = DateTime.Today.AddDays(7)
            });
        }

        [HttpPost]
        public IActionResult Create(Offer model)
        {
            if (model.End_Date < model.Start_Date)
                ModelState.AddModelError("End_Date", "End Date cannot be before Start Date.");

            if (ModelState.IsValid)
            {
                _repo.AddOffer(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var offer = _repo.GetOfferById(id);
            if (offer == null) return NotFound();
            return View(offer);
        }

        [HttpPost]
        public IActionResult Edit(Offer model)
        {
            if (model.End_Date < model.Start_Date)
                ModelState.AddModelError("End_Date", "End Date cannot be before Start Date.");

            if (ModelState.IsValid)
            {
                _repo.UpdateOffer(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var offer = _repo.GetOfferById(id);
            if (offer == null) return NotFound();
            return View(offer);
        }

        public IActionResult Delete(int id)
        {
            _repo.DeleteOffer(id);
            return RedirectToAction("Index");
        }

        public IActionResult ActivateOffer(int id)
        {
            _repo.ActivateOffer(id);
            return RedirectToAction("Index");
        }

        public IActionResult DeactivateOffer(int id)
        {
            _repo.DeactivateOffer(id);
            return RedirectToAction("Index");
        }
    }
}