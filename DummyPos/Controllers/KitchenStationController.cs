using DummyPos.Models;
using DummyPos.Repositories.Implementation;
using DummyPos.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DummyPos.Controllers
{
    public class KitchenStationController : Controller
    {
        private readonly IKitchenStationRepo _repo;
        public KitchenStationController(IKitchenStationRepo repo)
        {
            _repo = repo;
        }

        // branchId comes from the Branch Management list
        public IActionResult Index(int branchId)
        {
            ViewBag.BranchId = branchId;
            return View(_repo.GetStationsByBranchId(branchId));
        }
        [HttpGet]
        public IActionResult Create(int branchId)
        {
            return View(new KitchenStation { Branch_Id = branchId });
        }

        [HttpPost]
        public IActionResult Create(KitchenStation model)
        {
            if (ModelState.IsValid)
            {
                _repo.AddStation(model);
                return RedirectToAction("Index", new { branchId = model.Branch_Id });
            }
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var station = _repo.GetStationById(id);
            if (station == null) return NotFound();
            return View(station);
        }

        [HttpPost]
        public IActionResult Edit(KitchenStation model)
        {
            if (ModelState.IsValid)
            {
                _repo.UpdateStation(model);
                return RedirectToAction("Index", new { branchId = model.Branch_Id });
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var station = _repo.GetStationById(id);
            _repo.DeleteStation(id);
            return RedirectToAction("Index", new { branchId = station.Branch_Id });
        }
        public IActionResult Details(int id)
        {
            var details = _repo.GetStationById(id);

            if (details == null)
            {
                return NotFound();
            }

            return View(details);
        }

        /* public IActionResult Toggle(int id, bool status)
         {
             var station = _repo.GetStationById(id);
             _repo.ToggleActive(id, status);
             return RedirectToAction("Index", new { branchId = station.Branch_Id });
         }*/
        public IActionResult DeactivateKS(int id)
        {
            _repo.DeactivateK_S(id);
            return RedirectToAction("Index");
        }

        // Action for Activate button
        public IActionResult ActivateKS(int id)
        {
            _repo.ActivateK_S(id);
            return RedirectToAction("Index");
        }
    }
}