using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WEBSITENEWS.Models;

namespace WEBSITENEWS.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class DefaultController : Controller
    {
        NewsContext db;
        public DefaultController(NewsContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
           int Teammembercount= db.Teammember.Count();
           int Newscount = db.News.Count();
           int Categorycount = db.Category.Count();
           int Contactscount = db.Contacts.Count();
           return View(new AdminNumbers { team=Teammembercount,news=Newscount,Cate=Categorycount,Messges=Contactscount});
        }
    }

  public  class AdminNumbers
    {
        public int Messges { get; set; }
        public int Cate { get; set; }
        public int team { get; set; }
        public int news { get; set; }
    }
}