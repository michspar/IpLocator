﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IpLocator.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IpSearch()
        {
            return PartialView();
        }

        public ActionResult CitySearch()
        {
            return PartialView();
        }
    }
}