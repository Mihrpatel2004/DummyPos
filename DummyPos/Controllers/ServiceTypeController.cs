using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DummyPos.Controllers
{
    public class ServiceTypeController : Controller
    {
        private readonly IServiceTypeRepo _serviceRepo;

        // Constructor injection
        public ServiceTypeController(IServiceTypeRepo serviceRepo)
        {
            _serviceRepo = serviceRepo;
        }
        public IActionResult Index()
        {
            var list = _serviceRepo.GetAllServiceTypes();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ServiceType { Is_Active = true }); // Return empty model to the view
        }
        [HttpPost]
        public IActionResult Create(ServiceType model)
        {
            if (ModelState.IsValid)
            {
                _serviceRepo.AddServiceType(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public IActionResult Details(int id)
        {
            var service = _serviceRepo.GetServiceTypeById(id);
            if (service == null) return NotFound();
            return View(service);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var service = _serviceRepo.GetServiceTypeById(id);
            if (service == null) return NotFound();
            return View(service);
        }

        [HttpPost]
        public IActionResult Edit(ServiceType model)
        {
            if (ModelState.IsValid)
            {
                _serviceRepo.UpdateServiceType(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public IActionResult ActivateService(int id)
        {
            _serviceRepo.ActivateServiceType(id);
            return RedirectToAction("Index");
        }

        public IActionResult DeactivateService(int id)
        {
            _serviceRepo.DeactivateServiceType(id);
            return RedirectToAction("Index");
        }
    }
}
