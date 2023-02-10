using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoDB2.Models;

namespace DemoDB2.Controllers
{
   
    public class AdminController : Controller
    {
        DBSportStoreEntities db = new DBSportStoreEntities();
        // GET: Admin
        List<AdminUser> students = new List<AdminUser>() {
                new AdminUser(){ ID = 123, PasswordUser="Bill"},
            };

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginAccount(AdminUser _user)
        {
            var check = db.AdminUsers.Where(s => s.ID == _user.ID && s.PasswordUser == _user.PasswordUser).FirstOrDefault();
            if(check == null)
            {
                ViewBag.Error = "Sai info";
                return View("Index");
            }
            else
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                Session["ID"] = _user.ID;
                Session["Password"] = _user.PasswordUser;
                return RedirectToAction("Index", "Product");
            }
        }
    }
}