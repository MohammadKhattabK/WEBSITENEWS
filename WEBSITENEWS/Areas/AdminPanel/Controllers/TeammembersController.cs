using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using WEBSITENEWS.Models;
using System.IO;

namespace WEBSITENEWS.Controllers
{
    [Area("AdminPanel")]
    public class TeammembersController : Controller
    {
        private readonly NewsContext db;
        private IHostingEnvironment host;
        public TeammembersController(NewsContext context, IHostingEnvironment hostEnv)
        {
            db = context;
            host = hostEnv;
        }

        // GET: Teammembers
        public async Task<IActionResult> Index()
        {
            return View(await db.Teammember.ToListAsync());
        }

        // GET: Teammembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teammember = await db.Teammember
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teammember == null)
            {
                return NotFound();
            }

            return View(teammember);
        }

        // GET: Teammembers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teammembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teammember teammember)
        {
            if (ModelState.IsValid)
            {
                uploadphoto(teammember);
                db.Add(teammember);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teammember);
        }

        void uploadphoto(Teammember model)
        {
            if (model.File != null)
            {
                string uploadsFolder = Path.Combine(host.WebRootPath, "Images/News");
                string uniqueFileName = Guid.NewGuid() + ".jpg";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    model.File.CopyTo(filestream);
                }
                model.Image = uniqueFileName;
            }
        }

        // GET: Teammembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teammember = await db.Teammember.FindAsync(id);
            if (teammember == null)
            {
                return NotFound();
            }
            return View(teammember);
        }

        // POST: Teammembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Teammember teammember)
        {
            if (id != teammember.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    uploadphoto(teammember);
                    db.Update(teammember);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeammemberExists(teammember.Id))
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
            return View(teammember);
        }

        // GET: Teammembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teammember = await db.Teammember
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teammember == null)
            {
                return NotFound();
            }

            return View(teammember);
        }

        // POST: Teammembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teammember = await db.Teammember.FindAsync(id);
            db.Teammember.Remove(teammember);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeammemberExists(int id)
        {
            return db.Teammember.Any(e => e.Id == id);
        }
    }
}
