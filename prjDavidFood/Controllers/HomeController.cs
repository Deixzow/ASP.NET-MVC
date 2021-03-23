using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using prjDavidFood.Models;

namespace prjDavidFood.Controllers
{
    public class HomeController : Controller
    {
        dbFoodEntities db = new dbFoodEntities();

        // GET: Home
        public ActionResult Index()
        {
            var foods = db.tFood.OrderByDescending(m => m.Id).ToList();
            return View(foods);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(tFood f)
        {
            tFood food = new tFood();
            food.name = f.name;
            food.price = f.price;
            food.category = f.category;
            food.PId = f.PId;
            db.tFood.Add(food);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        public ActionResult Edit(int Id)
        {
            var foods = db.tFood.Where(m => m.Id == Id).FirstOrDefault();
            return View(foods);
        }


        [HttpPost]
        public ActionResult Edit(tFood f)
        {
            var foods = db.tFood.Where(m => m.Id == f.Id).FirstOrDefault();
            foods.name = f.name;
            foods.price = f.price;
            foods.category = f.category;
            foods.PId = f.PId;
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        public ActionResult Delete(int Id)
        {
            var foods = db.tFood.Where(m => m.Id == Id).FirstOrDefault();
            db.tFood.Remove(foods);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Details(int Id)
        {
            var foods = db.tFood.Where(m => m.Id == Id).FirstOrDefault();
            return View(foods);
        }


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(tMember vMember)
        {
            string account = vMember.Uid;
            var member = db.tMember.Where(m => m.Uid == account).FirstOrDefault();
            if (member != null)
            {
                ViewBag.Msg = "帳號已經有人使用！";
            }
            else
            {
                db.tMember.Add(vMember);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(string Uid, string Pwd)
        {
            var user = db.tMember.Where(m => m.Uid == Uid && m.Pwd == Pwd).FirstOrDefault();
            var admin = db.tAdmin.Where(m => m.Uid == Uid && m.Pwd == Pwd).FirstOrDefault();

            if (user != null)
            {
                Session["MemberLevel"] = "1";
                FormsAuthentication.RedirectFromLoginPage(user.Uid, true);
                return RedirectToAction("Index", "Member");
            }
            else if (admin!=null)
            {
                Session["MemberLevel"] = "2";
                FormsAuthentication.RedirectFromLoginPage(admin.Uid, true);
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.Msg="帳號或密碼錯誤，請重新輸入！";
            }

            return View();
        }













        //[HttpPost]
        //public ActionResult Login(string Uid, string Pwd)
        //{
        //    var member = db.tMember.Where(m => m.Uid == Uid && m.Pwd == Pwd).FirstOrDefault();

        //    if (member != null)
        //    {
        //        FormsAuthentication.RedirectFromLoginPage(member.Uid, true);
        //        return RedirectToAction("Index", "Member");
        //    }
        //    else
        //    {
        //        ViewBag.Msg = "帳號或密碼錯誤，請重新輸入！";
        //    }
        //    return View();
        //}
    }
}
