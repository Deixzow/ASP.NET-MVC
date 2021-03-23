using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using prjDavidFood.Models;

namespace prjDavidFood.Controllers
{
    public class MemberController : Controller
    {
        dbFoodEntities db = new dbFoodEntities();

        [Authorize]
        public ActionResult Index()
        {
            string level = Session["MemberLevel"].ToString();
            if (level != "1")
            {
                ViewBag.Msg = "帳號或密碼錯誤，請重新輸入！";
                RedirectToAction("Index", "Home");
            }


            var foods = db.tFood.OrderByDescending(m => m.Id).ToList();
            return View(foods);
        }

        /*-------------------       ShoppingCar       -------------------*/

        [Authorize]
        public ActionResult ShoppingCar(string PId)
        {
            string account = User.Identity.Name;
            var car = db.tShoppingCar.Where(m => m.Uid == account && m.PId == PId).FirstOrDefault(); ;
            if (car != null)
            {
                car.Amount += 1;
            }
            else
            {
                var product = db.tFood.Where(m => m.PId == PId).FirstOrDefault();
                tShoppingCar newCar = new tShoppingCar();
                newCar.Uid = account;
                newCar.PId = PId;
                newCar.Name = product.name;
                newCar.Price = product.price;
                newCar.Amount = 1;
                db.tShoppingCar.Add(newCar);
            }
            db.SaveChanges();
            return RedirectToAction("ShoppingCarList");
        }


        [Authorize]
        public ActionResult ShoppingCarList()
        {
            string account = User.Identity.Name;
            var product = db.tShoppingCar.Where(m => m.Uid == account).ToList();
            return View(product);
        }


        [Authorize]
        public ActionResult ShoppingCarAddAmount(int Id)
        {
            var product = db.tShoppingCar.Where(m => m.Id == Id).FirstOrDefault();
            product.Amount += 1;
            db.SaveChanges();

            return RedirectToAction("ShoppingCarList");
        }


        [Authorize]
        public ActionResult ShoppingCarSubAmount(int Id)
        {
            var product = db.tShoppingCar.Where(m => m.Id == Id).FirstOrDefault();
            product.Amount -= 1;
            db.SaveChanges();

            return RedirectToAction("ShoppingCarList");
        }


        [Authorize]
        public ActionResult ShoppingCarDelete(int Id)
        {
            var product = db.tShoppingCar.Where(m => m.Id == Id).FirstOrDefault();
            db.tShoppingCar.Remove(product);
            db.SaveChanges();

            return RedirectToAction("ShoppingCarList");
        }

        /*-------------------       Order       -------------------*/

        [Authorize]
        [HttpPost]
        public ActionResult Order(string Receiver, int ReceiverPhone, string ReceiverAddress)
        {
            //將ShoppingCar的東西，新增到Order
            string uid = User.Identity.Name;
            tOrder order = new tOrder();
            order.Uid = uid;
            order.Receiver = Receiver;
            order.ReceriverPhone = ReceiverPhone;
            order.RecerverAddress = ReceiverAddress;
            order.ReceiverDate = DateTime.Now;
            order.RecerverState = "未出貨";
            db.tOrder.Add(order);
            db.SaveChanges();

            //在清除ShoppingCar之前，也新增好OrderDetails
            int orderId = db.tOrder.OrderByDescending(m => m.OrderId).FirstOrDefault().OrderId;
            var car = db.tShoppingCar.Where(m => m.Uid == uid).ToList();
            tOrderDetails[] details = new tOrderDetails[car.Count];
            for (int i=0; i<details.Length; i++)
            {
                details[i] = new tOrderDetails();  //有待瞭解
                details[i].OrderId = orderId;
                details[i].PId = car[i].PId;
                details[i].Name = car[i].Name;
                details[i].Price = car[i].Price;
                details[i].Amount = car[i].Amount;
            }
            db.tOrderDetails.AddRange(details);
            db.tShoppingCar.RemoveRange(car);  //RemoveRange的部分待瞭解
            db.SaveChanges();

            return RedirectToAction("OrderList");
        }

        [Authorize]
        public ActionResult OrderList()
        {
            string uid = User.Identity.Name;
            var Order = db.tOrder.Where(m => m.Uid == uid).OrderByDescending(m=>m.OrderId).ToList();
            return View(Order);
        }


        [Authorize]
        public ActionResult OrderDetails(int OrderId)
        {
            var order = db.tOrderDetails.Where(m => m.OrderId == OrderId).ToList();
            return View(order);
        }

        /*-------------------       CRUD       -------------------*/
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }


        [Authorize]
        [HttpPost]
        public ActionResult Create(tFood food)
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
            var food = db.tFood.Where(m => m.Id == Id).FirstOrDefault();
            return View(food);
        }


        [Authorize]
        public ActionResult Edit(int Id)
        {
            var food = db.tFood.Where(m => m.Id == Id).FirstOrDefault();
            return View(food);
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
            var food = db.tFood.Where(m => m.Id == Id).FirstOrDefault();
            db.tFood.Remove(food);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [Authorize]
        public ActionResult Logout()
        {
            {
                FormsAuthentication.SignOut();
                Session.Abandon();   //消除快取
                return RedirectToAction("Index", "Home");
            }
        }
    }
}