using Flightos.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flightos.Models;
using System.Security.Cryptography.Xml;
using static System.Data.Entity.Infrastructure.Design.Executor;
using System.Web.UI;

namespace Flightos.Controllers
{
    public class HomeController : Controller
    {

        private FlightsDal db = new FlightsDal();
        private UserDal db2 = new UserDal();
        public ActionResult Index()
        {
            return View(db.Flights.ToList());
        }
        public ActionResult Logout()
        {
            Session["admin"] = null;
            Session["username"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Profile(string username)
        {
            Users user = db2.user.Where(x => x.username == username).FirstOrDefault();
            return View(user);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult submit(FormCollection form)
        {
            var twoway = form["twoway"];

            DateTime fromdate = DateTime.Today;
            DateTime todate = new DateTime(2050,12,30);
            var from = form["from"];
            var to = form["to"];
            var sortBy = form["sortBy"];
            int minprice = int.Parse(form["MinPrice"]);
            int maxprice = int.Parse(form["MaxPrice"]);
            var MyResult = db.Flights.ToList();
            //init datetime
            if (form["fromdate"] != "")
            {
                fromdate = new DateTime(int.Parse(form["fromdate"].Substring(0, 4)), int.Parse(form["fromdate"].Substring(5, 2)), int.Parse(form["fromdate"].Substring(8, 2)));
            }
            if (form["todate"] != "")
            {
                todate = new DateTime(int.Parse(form["todate"].Substring(0, 4)), int.Parse(form["todate"].Substring(5, 2)), int.Parse(form["todate"].Substring(8, 2)));
            }
            //clean outdated flights and flights with no seats
            MyResult = MyResult.Where(o => o.seats > 0).ToList().Where(o=> (o.depdate - DateTime.Today).Days>=0).ToList() ;
            if (twoway == "true")
            {
                //-------------- init tupels------------------
                var tuple_flights = new List<Tuple<Flights, Flights>>();
                foreach(var item in MyResult)
                {
                    foreach(var item2 in MyResult)
                    {
                        if (item.from == item2.to && item.to == item2.from &&
                           (item2.arrivedate- item.depdate).Days >0 )
                        {
                            tuple_flights.Add(new Tuple<Flights, Flights>(item, item2));
                        }
                    }
                    
                }
                //----------------filter logic------------------
                if (!(from == "None" && to == "None"))
                {
                    if (from == "None" || to == "None")
                    {
                        if (from == "None")
                        {
                            tuple_flights = tuple_flights.Where(x => x.Item1.to == to).ToList();
                        }
                        else if (to == "None")
                        {
                            tuple_flights = tuple_flights.Where(x => x.Item1.from == from).ToList();
                        }
                    }

                    else if (from != "None" && to != "None")
                    {
                        tuple_flights = tuple_flights.Where(x => x.Item1.from == from &&
                                                      x.Item1.to == to).ToList();
                    }
                }
                //--------------------sort logic------------------------
                if (sortBy == "price increase")
                {
                    tuple_flights = tuple_flights.OrderBy(p => p.Item1.price + p.Item2.price).ToList();
                }
                else if (sortBy == "price decrease")
                {
                    tuple_flights = tuple_flights.OrderByDescending(p => p.Item1.price + p.Item2.price).ToList();
                }
                else if (sortBy == "most popular")
                {
                    tuple_flights = tuple_flights.Where(x => x.Item1.seats + x.Item2.seats < 100).ToList();
                }
                tuple_flights = tuple_flights.Where(o => o.Item1.price + o.Item2.price >= minprice && o.Item1.price + o.Item2.price <= maxprice).ToList();
                tuple_flights = tuple_flights.Where(o => o.Item1.depdate >= fromdate && o.Item2.arrivedate <= todate).ToList();
                return View("submit2", tuple_flights);

            }
            if (!(from == "None" && to == "None"))
            { 
                if (from == "None" || to == "None") {
                    if (from == "None")
                    {
                        MyResult = MyResult.Where(x => x.to == to).ToList();
                    }
                    else if(to == "None")
                    {
                        MyResult = MyResult.Where(x => x.from == from).ToList();
                    }
                }
                
                else if (from != "None" && to != "None"){ 
                    MyResult = MyResult.Where(x => x.from == from &&
                                                  x.to == to).ToList();
                }
            }
            if (sortBy == "price increase")
            {
                MyResult = MyResult.OrderBy(p => p.price).ToList();
            }
            else if (sortBy == "price decrease")
            {
                MyResult = MyResult.OrderByDescending(p => p.price).ToList();
            }

            else if (sortBy == "most popular")
            {
                MyResult = MyResult.Where(x => x.seats < 50).ToList();
            }

            MyResult = MyResult.Where(o => o.price >= minprice && o.price <= maxprice).ToList();
            MyResult = MyResult.Where(o => o.depdate >= fromdate && o.depdate <= todate).ToList();
            return View("submit", MyResult);
        }
    }
}