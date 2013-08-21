using OATS_Capstone.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace OATS_Capstone
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CheckUserLogin());
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CheckUserLogin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
            var actionName = filterContext.ActionDescriptor.ActionName.ToLower();
            if (!(controllerName.Contains("account") ||
                (controllerName.Contains("tests") && actionName.Contains("anonymousdotest"))
                ))
            {
                var authen = AuthenticationSessionModel.Instance();

                if (!(authen.IsAuthentication&&authen.User!=null))
                {
                    //send them off to the login page
                    var url = new UrlHelper(filterContext.RequestContext);
                    var loginUrl = url.Content("~/Account/Index");
                    filterContext.HttpContext.Response.Redirect(loginUrl, true);
                    filterContext.Result = new EmptyResult();
                    return;
                }
            }
        }
    }

}