using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Book_Shoppe.App_Start
{
    public class AdminAuthorizationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                if (Controllers.UserController.CurrentUser.RoleID <= 2)
                {
                    context.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "Controller", "user" }, { "Action", "LogIn" } });
                }
            }
            catch (Exception)
            {
                context.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "Controller", "user" }, { "Action", "LogIn" } });
            }

            base.OnActionExecuting(context);
        }
       
    }
}