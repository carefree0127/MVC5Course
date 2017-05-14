using System;
using System.Web.Mvc;//手動增加

namespace MVC5Course.Controllers
{
    public class SharedViewBagAttribute : ActionFilterAttribute //Attribute
    {
        //在Action套用Attribute的時候可用()給值
        public string MyProperty { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.Message = "OnActionExecuting-1";
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Controller.ViewBag.Message = "OnActionExecuted-2";
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            filterContext.Controller.ViewBag.Message = "OnResultExecuted-3";
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.Message = MyProperty;
        }
    }
}