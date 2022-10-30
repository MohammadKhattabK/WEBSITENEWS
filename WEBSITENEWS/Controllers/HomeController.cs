using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEBSITENEWS.Models;

namespace WEBSITENEWS.Controllers
{
    public class HomeController : Controller
    {

        NewsContext db;

        public HomeController(NewsContext context)
        {
            db = context;
        }


        public IActionResult Index()
        {
            var result = db.Category.ToList();

            return View(result);
        }

        public IActionResult Contact()
        {
           return View();
        }

        [HttpPost]
        public IActionResult SaveContact(ContactUs model)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Contact", model);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Teammembers()
        {
            return View(db.Teammember.ToList());
        }
        public IActionResult Messages()
        {
           return View(db.Contacts.ToList());
        }
        [Authorize]
        public IActionResult News(int id)
        {
             Category c=db.Category.Find(id);
            ViewBag.cat = c.Name;
            ViewData["cat"] = c.Name;
            var result= db.News.Where(x=>x.CategoryId==id).OrderByDescending(X=>X.Date).ToList();
            return View(result);
        }

        public IActionResult DeleteNews(int id)
        {
            var News=db.News.Find(id);
            db.News.Remove(News);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
