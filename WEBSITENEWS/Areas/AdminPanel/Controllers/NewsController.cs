using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using WEBSITENEWS.Models;

namespace WEBSITENEWS.Controllers
{
    [Area("AdminPanel")]
    public class NewsController : Controller
    {
        private readonly NewsContext db;
        private IHostingEnvironment host;

        public NewsController(NewsContext context,IHostingEnvironment hostEnv)
        {
            db = context;
            host = hostEnv;
        }

        // GET: News
        public IActionResult Index()
        {
            var newsContext = db.News.Include(n => n.Category);
            return View( newsContext.ToList());
        }

        // GET: News/Details/5
        public  IActionResult  Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news =  db.News
                .Include(n => n.Category)
                .FirstOrDefault(m => m.Id == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: News/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(db.Category, "Id", "Name");
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create( News news)
        {
            if (ModelState.IsValid)
            {
                uploadphoto(news);
                db.Add(news);
                 db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(db.Category, "Id", "Name", news.CategoryId);
            return View(news);
        }

        // GET: News/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news =  db.News.Find(id);
            if (news == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(db.Category, "Id", "Name", news.CategoryId);
            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Edit(int id, News news)
        {
            if (id != news.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    uploadphoto(news);
                    db.Update(news);
                     db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(db.Category, "Id", "Name", news.CategoryId);
            return View(news);
        }

        // GET: News/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news =  db.News
                .Include(n => n.Category)
                .FirstOrDefault(m => m.Id == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var news =  db.News.Find(id);
            db.News.Remove(news);
             db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
            return db.News.Any(e => e.Id == id);
        }

        void uploadphoto(News model)
        {
            if (model.File !=null)
            {
                string uploadsFolder = Path.Combine(host.WebRootPath,"Images/News");
                string uniqueFileName = Guid.NewGuid() + ".jpg";
                string filePath = Path.Combine(uploadsFolder,uniqueFileName);
                using (var filestream=new FileStream(filePath,FileMode.Create))
                {
                    model.File.CopyTo(filestream);
                }
                model.Image = uniqueFileName;
            }
        }
    }
}
