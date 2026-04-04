using DummyPos.Models;
using DummyPos.Repositories.Implementation;
using DummyPos.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DummyPos.Controllers
{
    public class ScreenController : Controller
    {
        private readonly IScreenRepo _repo;
        private readonly IKitchenStationRepo _ksRepo;

        public ScreenController(IScreenRepo repo, IKitchenStationRepo ksRepo)
        {
            _repo = repo;
            _ksRepo = ksRepo;
        }

        // stationId comes from the Kitchen Station list
        public IActionResult Index(int stationId)
        {
            var station = _ksRepo.GetStationById(stationId);

            ViewBag.StationId = stationId;
            ViewBag.StationName = station.Station_Name;
            ViewBag.BranchId = station.Branch_Id; // Used for the "Back" button

            return View(_repo.GetScreensByStationId(stationId));

        }

        [HttpGet]
        public IActionResult Create(int stationId)
        {
            ViewBag.ItemList = _repo.GetItemsDropdown();
            return View(new ItemDisplayScreen { Station_Id = stationId });
        }

        [HttpPost]
        public IActionResult Create(ItemDisplayScreen model)
        {
            if (ModelState.IsValid)
            {
                _repo.AddScreen(model);
                return RedirectToAction("Index", new { stationId = model.Station_Id });
            }
            ViewBag.ItemList = _repo.GetItemsDropdown();
            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var screen = _repo.GetScreenById(id);
            if (screen == null) return NotFound();

            ViewBag.ItemList = _repo.GetItemsDropdown();
            return View(screen);
        }

        [HttpPost]
        public IActionResult Edit(ItemDisplayScreen model)
        {
            if (ModelState.IsValid)
            {
                _repo.UpdateScreen(model);
                return RedirectToAction("Index", new { stationId = model.Station_Id });
            }
            ViewBag.ItemList = _repo.GetItemsDropdown();
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var screen = _repo.GetScreenById(id);
            _repo.DeleteScreen(id);
            return RedirectToAction("Index", new { stationId = screen.Station_Id });
        }

        public IActionResult Details(int id)
        {
            var details = _repo.GetScreenById(id);

            if (details == null)
            {
                return NotFound();
            }

            return View(details);
        }

       /* public IActionResult DeactivateScreen(int id)
        {
            _repo.DeactivateScreen(id);
            return RedirectToAction("Index");
        }

        // Action for Activate button
        public IActionResult ActivateScreen(int id)
        {
            _repo.ActivateScreen(id);
            return RedirectToAction("Index");
        }*/
        /*  public IActionResult Toggle(int id, bool status)
          {
              var screen = _repo.GetScreenById(id);
              _repo.ToggleActive(id, status);
              return RedirectToAction("Index", new { stationId = screen.Station_Id });
          }*/
    }
}