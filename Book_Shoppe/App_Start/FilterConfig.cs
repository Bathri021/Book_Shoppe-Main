using System.Web;
using System.Web.Mvc;

namespace Book_Shoppe
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute
            {
                View = "Error"
            }, 1);
            filters.Add(new HandleErrorAttribute(), 2);
        }
    }
}
