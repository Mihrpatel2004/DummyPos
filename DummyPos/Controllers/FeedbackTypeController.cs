using Microsoft.AspNetCore.Mvc;
using DummyPos.Models;
using DummyPos.Repositories.Interface;

namespace DummyPos.Controllers
{
    public class FeedbackTypeController : Controller
    {
        private readonly IFeedbackTypeRepo _repo;

        public FeedbackTypeController(IFeedbackTypeRepo repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            var list =_repo.GetAllFeedbackTypes();
            return View(list);
        }
        /* => View(_repo.GetAllFeedbackTypes());*/
        [HttpGet]
        public IActionResult Create()
        {
            return View(new FeedbackType());
        } 
        /*=> View(new FeedbackType());*/

        [HttpPost]
        public IActionResult Create(FeedbackType model)
        {
            if (ModelState.IsValid)
            {
                _repo.AddFeedbackType(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var ft = _repo.GetFeedbackTypeById(id);
            if (ft == null) return NotFound();
            return View(ft);
        }

        [HttpPost]
        public IActionResult Edit(FeedbackType model)
        {
            if (ModelState.IsValid)
            {
                _repo.UpdateFeedbackType(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var ft = _repo.GetFeedbackTypeById(id);
            if (ft == null) return NotFound();
            return View(ft);
        }

        public IActionResult Delete(int id)
        {
            _repo.DeleteFeedbackType(id);
            return RedirectToAction("Index");
        }
    }
}