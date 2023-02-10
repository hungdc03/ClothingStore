using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using DemoDB2.Models;

namespace DemoDB2.Controllers
{
    public class CategoriesController : Controller
    {
        WebDoAn1Entities database = new WebDoAn1Entities();
        
        // GET: Categories
        public ActionResult Index()
        {
            return View(database.Categories.ToList());
        }
        // CREATE: Categories
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Category cate)
        {
            try
            {
                database.Categories.Add(cate);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult Details(int id)
        {
            return View(database.Categories.Where(s => s.Id == id).FirstOrDefault());
        }
        public ActionResult Edit(int id)
        {
            return View(database.Categories.Where(s => s.Id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(int id, Category cate)
        {
            database.Entry(cate).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            return View(database.Categories.Where(s => s.Id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, Category cate)
        {
            try
            {
                cate = database.Categories.Where(s => s.Id == id).FirstOrDefault();
                database.Categories.Remove(cate);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [ChildActionOnly]
        public PartialViewResult CategoryPartial()
        {
            var cateList = database.Categories.ToList();
            return PartialView(cateList);
        }
    }
}