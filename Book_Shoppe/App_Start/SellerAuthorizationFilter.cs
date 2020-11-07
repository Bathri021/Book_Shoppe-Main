using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Book_Shoppe.App_Start
{
    public class SellerAuthorizationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                if (Book_Shoppe.Controllers.UserController.CurrentUser.RoleID != 1)
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "Controller", "user" }, { "Action", "LogIn" } });
                }
            }
            catch (Exception)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "Controller", "user" }, { "Action", "LogIn" } });
            }
            base.OnActionExecuting(filterContext);
        }
    }
}