using DemoDB2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DemoDB2.Controllers
{
    public class VouchersController : Controller
    {
        MainDB2 database = new MainDB2();
        // GET: Vouchers
        //Danh sách voucher
        public ActionResult Index()
        {
            return View(database.Vouchers.ToList());
        }
        //Tạo voucher
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProductID, Content, Value")] Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                database.Vouchers.Add(voucher);
                await database.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(voucher);
        }

        //Xóa mã giảm giá
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voucher voucher = database.Vouchers.Find(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }
            return View(voucher);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Voucher voucher = database.Vouchers.Find(id);
            database.Vouchers.Remove(voucher);
            database.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            return View(database.Vouchers.Where(s => s.ID == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Edit(int id, Voucher vouc)
        {
            database.Entry(vouc).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            return View(database.Vouchers.Where(s => s.ID == id).FirstOrDefault());
        }
    }
}