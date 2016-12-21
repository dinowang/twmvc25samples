using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace twmvc25.Controllers
{
    public class ViewTestController : Controller
    {
        // GET: VTest
        public ActionResult Index()
        {
            return View();
        }
    }
}