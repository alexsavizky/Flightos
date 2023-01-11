using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Flightos.Models;
using Flightos.Dal;
using System.IO;
using System.Security.Cryptography;

namespace Flightos.Controllers
{
    public class PayController : Controller
    {
        private PayDal db = new PayDal();
        private UserDal db2 = new UserDal();
        private FlightsDal db3 = new FlightsDal();
        // GET: Pay
        public ActionResult Index(string username, string flight_id, string flight_id2)
        {
            PayTemp pay = new PayTemp();
            pay.users = db2.user.Where(x => x.username == username).FirstOrDefault();
            pay.flight = db3.Flights.Where(x => x.flightnum == flight_id).FirstOrDefault();
            if (flight_id2 != "null")
            {
                pay.flightreturn = db3.Flights.Where(x => x.flightnum == flight_id2).FirstOrDefault();
            }
            else
            {
                pay.flightreturn = null;
            }
            return View(pay);
        }

        // Id flightnum uer_id card_num card_ccv validity
        [HttpPost]
        public ActionResult Index2(FormCollection form)
        {
            string flightnum = form["flightid"];
            
            // update seats 
            Flights temp = db3.Flights.Where(x => x.flightnum == flightnum).FirstOrDefault();
            temp.seats -= int.Parse(form["num of tickets"]);
            db3.Entry(temp).State = EntityState.Modified;
            db3.SaveChanges();
            // update seats for two way flight
            if (form["flightid2"] != "null")
            {
                string flightnum2 = form["flightid2"];
                temp = db3.Flights.Where(x => x.flightnum == flightnum2).FirstOrDefault();
                temp.seats -= int.Parse(form["num of tickets"]);
                db3.Entry(temp).State = EntityState.Modified;
                db3.SaveChanges();
            }

            //save payment on data base
            if (form["issave"] == "true")
            {

                var userid2 = form["username"];

                var newPay = new Pay()
                {
                    Id = db.Pay.ToList().OrderByDescending(o => o.Id).FirstOrDefault().Id + 1,
                    flightnum = flightnum,
                    uer_id = Convert.ToInt32(userid2),
                    card_num = Encrypt(form["ccn"].ToString()),
                    card_ccv = Convert.ToInt32((form["cvv"])),
                    validity = form["expireMM"] + form["expireYY"]
                };
                if (newPay != null)
                {
                    db.Pay.Add(newPay);
                    db.SaveChanges();
                }
                if (form["flightid2"] != "null")
                {
                    newPay = new Pay()
                    {
                        Id = db.Pay.ToList().OrderByDescending(o => o.Id).FirstOrDefault().Id + 1,
                        flightnum = form["flightid2"],
                        uer_id = Convert.ToInt32(userid2),
                        card_num = Encrypt(form["ccn"].ToString()),
                        card_ccv = Convert.ToInt32((form["cvv"])),
                        validity = form["expireMM"] + form["expireYY"]
                    };
                    if (newPay != null)
                    {
                        db.Pay.Add(newPay);
                        db.SaveChanges();
                    }

                }
            }
            TempData["msg"] = "<script>alert('The payment was successfull');</script>";
            return RedirectToAction("Index", "Home");
        }
        private string Encrypt(string textToEncrypt)
        {
            try
            {
 
                string ToReturn = "";
                string publickey = "santhosh";
                string secretkey = "engineer";
                byte[] secretkeyByte = { };
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    ToReturn = Convert.ToBase64String(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}