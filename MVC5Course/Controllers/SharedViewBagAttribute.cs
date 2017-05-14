using System;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class SharedViewBagAttribute : ActionFilterAttribute //Attribute
    {
        //可用()給值
        public string MyProperty { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.Message = "Your application description page.";
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Controller.ViewBag.Message = "Your application description page.";
        }
    }
}