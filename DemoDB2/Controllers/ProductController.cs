using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoDB2.Models;

namespace DemoDB2.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        DBSportStoreEntities database = new DBSportStoreEntities();
        public ActionResult Index()
        {
            return View(database.Products.ToList());
        }


    }
}