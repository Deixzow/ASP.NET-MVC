using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using prjDavidFood.Models;

namespace prjDavidFood.Controllers
{
    public class AdminController : Controller
    {
        dbFoodEntities db = new dbFoodEntities();


        [Authorize]
        public ActionResult Index()
        {
            var foods = db.tFood.OrderBy(m => m.Id).ToList();
            return View(foods);
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }


        /*--------------        CRUD       --------------*/

        [Authorize]
        public ActionResult FoodCreate()
        {
            return View();
        }
        
        [Authorize]
        [HttpPost]
        public ActionResult FoodCreate(tFood food)
        {
            tFood f = new tFood();
            f.name = food.name;
            f.price = food.price;
            f.category = food.category;
            f.PId = food.PId;
            db.tFood.Add(f);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        [Authorize]
        public ActionResult Details(int Id)
        {
            var product = db.tFood.Where(m => m.Id == Id).FirstOrDefault();
            return View(product);
        }


        [Authorize]
        public ActionResult Edit(int Id)
        {
            var product = db.tFood.Where(m => m.Id == Id).FirstOrDefault();
            return View(product);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(tFood food)
        {
            var f = db.tFood.Where(m => m.Id == food.Id).FirstOrDefault();
            f.name = food.name;
            f.price = food.price;
            f.category = food.category;
            f.PId = food.PId;
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        [Authorize]
        public ActionResult Delete(int Id)
        {
            var product = db.tFood.Where(m => m.Id == Id).FirstOrDefault();
            db.tFood.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        /*--------------        Order       --------------*/

        [Authorize]
        public ActionResult OrderList()
        {
            var order = db.tOrder.OrderByDescending(m => m.OrderId).ToList();
            return View(order);
        }

        [Authorize]
        public ActionResult OrderDetails(int OrderId)
        {
            var order = db.tOrderDetails.Where(m => m.OrderId == OrderId).ToList();
            return View(order);
        }


        [Authorize]
        public ActionResult GoGo(int OrderId)
        {
            var order = db.tOrder.Where(m => m.OrderId == OrderId).FirstOrDefault();
            order.RecerverState = "已出貨";
            db.SaveChanges();

            return RedirectToAction("OrderList");
        }

        [Authorize]
        public ActionResult Finish(int OrderId)
        {
            var order = db.tOrder.Where(m => m.OrderId == OrderId).FirstOrDefault();
            order.RecerverState = "已到貨";
            db.SaveChanges();

            return RedirectToAction("OrderList");
        }

        /*--------------        Member       --------------*/
        [Authorize]
        public ActionResult MemberList()
        {
            var member = db.tMember.OrderByDescending(m => m.Uid).ToList();
            return View(member);
        }

        [Authorize]
        public ActionResult MemberOrderList(string Uid)
        {
            string account = User.Identity.Name;
            var order = db.tOrder.Where(m => m.Uid == Uid).ToList();

            return View(order);
        }
    }
}
