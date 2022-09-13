using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HW7Project.Models;

namespace HW7Project.Controllers
{
    public class HomeController : Controller
    {
        HW7ProjectContext db = new HW7ProjectContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProductList()
        {
            var products = db.Products.Where(p => p.Discontinued == false).ToList();

            return View(products);
        }


        public ActionResult MyCart()
        {

            return View();
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(VMLogin vMLogin)
        {

            //select * from Members where account=@account and password=@password
            string pw = Members.getHashPassword(vMLogin.Password);
            var member = db.Members.Where(m => m.Account == vMLogin.Account && m.Password == pw).FirstOrDefault();

            if (member == null)
            {
                ViewBag.ErrMsg = "帳號或密碼有錯!!";
                return View(vMLogin);
            }

            Session["member"] = member;
            Session["memberID"] = member.MemberID;
            return RedirectToAction("ProductList");


        }


        [LoginCheck]
        public ActionResult Logout()
        {

            Session["member"] = null;
            Session["memberID"] = null;
            return RedirectToAction("ProductList", "Home");
        }

        [LoginCheckMemebers]
        public ActionResult CheckList()
        {
            int membersID = Int16.Parse((Session["memberID"].ToString()));
            var list = db.Orders.Where(p => p.MemberID == membersID).ToList();
            return View(list);
        }

    }
}