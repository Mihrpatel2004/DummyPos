using Microsoft.AspNetCore.Mvc;
using DummyPos.Models;
using DummyPos.Repositories.Interface;

namespace DummyPos.Controllers
{
    public class PaymentTypeController : Controller
    {
        private readonly IPaymentTypeRepo _repo;

        public PaymentTypeController(IPaymentTypeRepo repo)
        {
            _repo = repo;
        }

        // INDEX: Show List
        public IActionResult Index()
        {
            var list= _repo.GetAllPaymentTypes();
            return View(list);  
        }

        // CREATE: Get and Post
        [HttpGet]
        public IActionResult Create()
        {
            return View(new PaymentType { Is_Active = true});
        }

        [HttpPost]
        public IActionResult Create(PaymentType model)
        {
            if (ModelState.IsValid)
            {
                _repo.AddPaymentType(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // EDIT: Get and Post
        public IActionResult Edit(int id)
        {
            var pt = _repo.GetPaymentTypeById(id);
            if (pt == null) return NotFound();
            return View(pt);
        }

        [HttpPost]
        public IActionResult Edit(PaymentType model)
        {
            if (ModelState.IsValid)
            {
                _repo.UpdatePaymentType(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // DETAILS
        public IActionResult Details(int id)
        {
            var pt = _repo.GetPaymentTypeById(id);
            if (pt == null) return NotFound();
            return View(pt);
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            _repo.DeletePaymentType(id);
            return RedirectToAction("Index");
        }

        // ACTIVATE / DEACTIVATE
        public IActionResult ActivatePayment(int id)
        {
            _repo.ActivatePaymentType(id);
            return RedirectToAction("Index");
        }

        public IActionResult DeactivatePayment(int id)
        {
            _repo.DeactivatePaymentType(id);
            return RedirectToAction("Index");
        }
    }
}