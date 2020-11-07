using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Book_Shoppe.App_Start
{
    public class CustomeErrorHandler : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.ExceptionHandled)
            {
                string controllerName = (string)exceptionContext.RouteData.Values["controller"];
                string actionName = (string)exceptionContext.RouteData.Values["action"];

                Exception custException = new Exception("There is some error");


                var model = new HandleErrorInfo(custException, controllerName, actionName);

                exceptionContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/Error.cshtml",
                    ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                };

                exceptionContext.ExceptionHandled = true;
            }
        }
      
    }
}