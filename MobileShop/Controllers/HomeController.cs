using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobileShop.Models;
using System.Data.Entity;
using System.Net;

namespace MobileShop.Controllers
{
    public class HomeController : Controller
    {
        private DemoEntities db = new DemoEntities();
        public ActionResult Index(string searchString , string mMobileName)
        {
            List<string> genreList = new List<string>();
            var genreQuery = from g in db.Mobiles
                             orderby g.MobileName
                             select g.MobileName;
            genreList.AddRange(genreQuery.Distinct());
            ViewBag.mMobileName = new SelectList(genreList);


            var mobiles = from m in db.Mobiles
                         select m;
            if (!String.IsNullOrEmpty(mMobileName))
            {
                mobiles = mobiles.Where(x => x.MobileName == mMobileName);
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                mobiles = mobiles.Where(x => x.MobileName.Contains(searchString));
            }
            return View(mobiles);
        }
        
       
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Mobile mobile)
        {
            if (ModelState.IsValid)
            {
                db.Mobiles.Add(mobile);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(mobile);
        }

        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mobile mobile = db.Mobiles.Find(id);

            if (mobile == null)
            {
                return HttpNotFound();
            }
            return View(mobile);
        }
        [HttpPost]
        public ActionResult Edit(Mobile mobile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mobile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mobile);
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mobile mobile = db.Mobiles.Find(id);
            if(mobile == null)
            {
                return HttpNotFound();
            }
            return View(mobile);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mobile mobile = db.Mobiles.Find(id);

            if(mobile == null)
            {
                return HttpNotFound();
            }
            return View(mobile);
        }
        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Mobile mobile = db.Mobiles.Find(id);
            db.Mobiles.Remove(mobile);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}