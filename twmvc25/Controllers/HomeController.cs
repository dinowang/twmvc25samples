using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using twmvc25.Models;

namespace twmvc25.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Setup()
        {
            var model = new TestingModel
            {
                Preview = Request.Cookies.AllKeys.Contains("Preview"),
                Theme = Request.Cookies["Theme"]?.Value
            };

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Setup(TestingModel model)
        {
            var previewCookie = new HttpCookie("Preview") { Value = model.Preview.ToString() };
            var themeCookie = new HttpCookie("Theme") { Value = model.Theme };

            if (!model.Preview)
            {
                previewCookie.Expires = DateTime.Now.AddMinutes(-1);
            }

            if (string.IsNullOrEmpty(model.Theme))
            {
                themeCookie.Expires = DateTime.Now.AddMinutes(-1);
            }

            Response.SetCookie(previewCookie);
            Response.SetCookie(themeCookie);

            if (Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }

            return RedirectToAction("Index", "Home");
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
    }
}