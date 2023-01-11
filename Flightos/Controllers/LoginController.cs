using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flightos.Dal;
using Flightos.Models;

namespace Flightos.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "loginUsername,loginPassword")] FormCollection form)
        {
            var username = form["loginUsername"];
            var pass = form["loginPassword"];

            var db = new UserDal();
            var user = db.user.Where(x => x.username == username).FirstOrDefault();

            if (user == null)
            {
                TempData["msg"] = "<script>alert('Invalid username or password');</script>";
                return RedirectToAction("Index");
            }

            if (user.password.Equals(pass))
            {
                if (db.Admin.Find(Convert.ToInt32(user.ID)) != null)
                    Session["admin"] = true;

                Session["username"] = user.username;
            }
            else
            {
                TempData["msg"] = "<script>alert('Invalid username or password');</script>";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(
            [Bind(Include = "registerID,registerPassword,registerFName, registerLName, registerUsername")]
            FormCollection form)
        {
            var id = Convert.ToInt32(form["registerID"]);
            var pass = form["registerPassword"];

            var db = new UserDal();
            var user = db.user.Find(id);
            if (user != null)
                return RedirectToAction("index");

            var newUser = new Users()
            {
                ID = id,
                password = pass,
                fname = form["registerFName"],
                lname = form["registerLName"],
                username = form["registerUsername"]
            };

            db.user.Add(newUser);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}