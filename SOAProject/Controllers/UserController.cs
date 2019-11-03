﻿using System;
using System.Collections.Generic;
using System.Linq;
using SOAProject.Models;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace SOAProject.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [HttpGet]
        public ActionResult Login()
        {
            for (int i = 0; i < 10; i++)
            {
                BaseObject.Session += Guid.NewGuid().ToString().Replace("-", "");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection form)
        {
            var tc = form["TCNo"];
            var password = form["Password"];

            if (Login(tc, password))
            {
                
                return RedirectToAction("Index","Home");
            }
            return RedirectToAction("Index");
        }
        
        public ActionResult Register()
        {
            return View();
        }

        public bool Login(string TC,string Password)
        {
            var result = ApiConnect.Request("/login", new Dictionary<string, string>
                {
                    { "TC",TC},
                    {"Password",Password}

                }
            );

            int res = JsonConvert.DeserializeObject<int>(result.ToString()); 
            if(res == 1)
            {
                //AddCookie("access_token",res.Token);
                //AddCookie("User_ID", res.User_ID);

                return true;
            }
            else
            {
                return false;
            }
           
        }

        public void AddCookie(string cookieName,string cookieValue)
        {
            HttpCookie cookie = new HttpCookie(cookieName, cookieValue);
            cookie.Expires = DateTime.Now.AddYears(1);
            HttpContext.Response.Cookies.Add(cookie);
        }    
    }
}