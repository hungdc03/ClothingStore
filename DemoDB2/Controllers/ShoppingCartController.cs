using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoDB2.Models;

namespace DemoDB2.Controllers
{
    public class ShoppingCartController : Controller
    {
        MainDB2 db = new MainDB2();
        // GET: ShoppingCart
        public ActionResult ShowCart()
        {
            if (Session["Cart"] == null)
                return View("ShowCart");
            Cart _cart = Session["Cart"] as Cart;
            return View(_cart);
        }
        // Tạo mới giỏ hàng, nguồn được lấy từ Session
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        // Thêm sản phẩm vào giỏ hàng
        public ActionResult AddToCart(int id)
        {
            // lấy sản phẩm theo id
            var _pro = db.Products.SingleOrDefault(s => s.ProductID == id);
            if (_pro != null)
            {
                GetCart().Add_Product_Cart(_pro);
            }
            return RedirectToAction("ProductList", "Products");
        }
        // Cập nhật số lượng và tính lại tổng tiền
        public ActionResult Update_Cart_Quantity(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int id_pro = int.Parse(Request.Form["idPro"]);
            int _quantity = int.Parse(Request.Form["carQuantity"]);
            cart.Update_quantity(id_pro, _quantity);

            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_CartItem(id);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        // Tính tổng số lượng đơn hàng
        public PartialViewResult BagCart()
        {
            int total_quantity_item = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
                total_quantity_item = cart.Total_quantity();
            ViewBag.QuantityCart = total_quantity_item;
            return PartialView("BagCart");
        }
        // Các phương thức cho thanh toán
        public ActionResult CheckOut(FormCollection form)
        {
            try
            {
                Cart cart = Session["Cart"] as Cart;
                OrderPro _order = new OrderPro();
                _order.DateOrder = DateTime.Now;
                _order.AddressDeliverry = form["AddressDeliverry"];
                _order.IDCus = int.Parse(form["CodeCustomer"]);
                db.OrderProes.Add(_order);
                foreach (var item in cart.Items)
                {
                    // lưu dòng sản phẩm vào chi tiết hóa đơn
                    OrderDetail _order_detail = new OrderDetail();
                    _order_detail.IDOrder = _order.ID;
                    _order_detail.IDProduct = item._product.ProductID;
                    _order_detail.UnitPrice = (double)item._product.Price;
                    _order_detail.Quantity = item._quantity;
                    db.OrderDetails.Add(_order_detail);
                    //Xử lý cập nhật lại số lượng tồn trong bảng
                    foreach(var p in db.Products.Where(s => s.ProductID == _order_detail.IDProduct))
                    {
                        var update_quan_pro = p.Quantity - item._quantity;
                        p.Quantity = update_quan_pro;
                    }    
                }
                db.SaveChanges();
                cart.ClearCart();
                return RedirectToAction("CheckOut_Success", "ShoppingCart");
            }
            catch
            {
                return Content("Có sai sót! Xin kiểm tra lại thông tin");
            }
        }
        // Thông báo thanh toán thành công
        public ActionResult CheckOut_Success()
        {
            return View();
        }

        //Giảm giá
        public ActionResult SalePrice(string value = "1", decimal id = 0)
        {
            Cart cart = Session["Cart"] as Cart;

            decimal total_sale = decimal.Parse(value);
            cart.Update_voucher((int)id, total_sale);

            return RedirectToAction("ShowCart", "ShoppingCart");         
        }

        //Lựa chọn mã giảm giá
        [ChildActionOnly]
        public ActionResult SelectVoucher(int? id)
        {
            List<Voucher> item = db.Vouchers.ToList();
            ViewBag.Select = new SelectList(item.Where(s => s.ProductID == id), "Value", "Content", 1);

            return PartialView();
        }
    }
}