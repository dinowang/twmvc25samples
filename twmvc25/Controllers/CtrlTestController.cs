using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace twmvc25.Controllers
{
    public class CtrlTestController : Controller
    {
        protected override IActionInvoker CreateActionInvoker()
        {
            return new AlternativeActionInvoker();
        }

        [PreviewAction("IndexPreview")]
        public ActionResult Index()
        {
            ViewBag.Message = "Hello! This is Index speaking";

            return View();
        }

        [ChildActionOnly]
        public ActionResult IndexPreview()
        {
            ViewBag.Message = "Hello! This is IndexPreview speaking";

            return View();
        }
    }

    public class AlternativeActionInvoker : ControllerActionInvoker
    {
        protected override ActionResult InvokeActionMethod(
            ControllerContext controllerContext, 
            ActionDescriptor actionDescriptor,
            IDictionary<string, object> parameters)
        {
            var actualActionDescriptor = actionDescriptor;
            var alternatives = actionDescriptor
                                    .GetCustomAttributes(typeof(AlternativeActionAttribute), true)
                                    .Cast<AlternativeActionAttribute>();

            foreach (var alternative in alternatives)
            {
                if (alternative.Evaluate(controllerContext))
                {
                    if (alternative.OverrideRouteValue)
                    {
                        var routeValues = controllerContext.RequestContext.RouteData.Values;
                        routeValues["action"] = alternative.AlternativeActionName;
                    }

                    actualActionDescriptor = FindAction(controllerContext, actionDescriptor.ControllerDescriptor, alternative.AlternativeActionName);
                    break;
                }
            }

            return base.InvokeActionMethod(controllerContext, actualActionDescriptor, parameters);
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public abstract class AlternativeActionAttribute : Attribute
    {
        public string AlternativeActionName { get; private set; }

        public bool OverrideRouteValue { get; set; }

        public AlternativeActionAttribute(string alternateActionName)
        {
            AlternativeActionName = alternateActionName;
        }

        public abstract bool Evaluate(ControllerContext controllerContext);
    }

    public class PreviewActionAttribute : AlternativeActionAttribute
    {
        public PreviewActionAttribute(string alternateActionName) : base(alternateActionName)
        {
        }

        public override bool Evaluate(ControllerContext controllerContext)
        {
            var request = controllerContext.RequestContext.HttpContext.Request;

            return request.Cookies.AllKeys.Contains("Preview");
        }
    }
}