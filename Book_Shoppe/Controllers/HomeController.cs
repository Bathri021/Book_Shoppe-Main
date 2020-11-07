using Book_Shoppe.BL;
using Book_Shoppe.DAL;
using Book_Shoppe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Book_Shoppe.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            BookBL bookContext = new BookBL();
            IEnumerable<Book> books = bookContext.GetBooks();
            return View(books);
        }

        public JsonResult GetSearchingData(string SearchValue)
       {
            BookBL bookContext = new BookBL();
            IEnumerable<Book> BookList = bookContext.SearchResult(SearchValue);
            return Json(BookList, JsonRequestBehavior.AllowGet);
           
        }
    }
}