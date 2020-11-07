using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Book_Shoppe.Startup))]
namespace Book_Shoppe
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           
        }
    }
}
