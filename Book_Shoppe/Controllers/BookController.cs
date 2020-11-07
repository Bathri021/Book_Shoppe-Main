using Book_Shoppe.DAL;
using Book_Shoppe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Book_Shoppe.BL;
using Book_Shoppe.Models;
using Book_Shoppe.App_Start;
using AutoMapper;

namespace Book_Shoppe.Controllers
{
    public class BookController : Controller
    {
        IBookBL IBookBL = new BookBL();

        // GET: Book 
        // Sellers Books Page
       // [SellerAuthorizationFilter]
        [Authorize(Roles ="Seller")]
        public ActionResult Index()
        {
            int userID = Convert.ToInt32(Session["UserID"]);
            IEnumerable<Book> Books = IBookBL.GetUserBooks(userID);
            return View(Books);
        }

        // Get & Show The User Order Books
        [Authorize(Roles ="Seller")]
        public ActionResult UserOrder()
        {
            int userID = Convert.ToInt32(Session["UserID"]);
            IEnumerable<Book> Books = IBookBL.GetUserOrdredBooks(userID);

            return View(Books);
        }

        // [AdminAuthorizationFilter]
        [Authorize(Roles ="Admin")]
        public ActionResult ManageBookCategory()
        {
            IEnumerable<Genre> Genres = IBookBL.GetAllGenres();
            return View(Genres);
        }

        [Authorize(Roles ="Admin")]
        public ActionResult ManageBookLanguage()
        {
            IEnumerable<Language> Languages = IBookBL.GetAllLanguages();
            return View(Languages);
        }
        // Master nav link Geners filter
        public ActionResult Geners(int id)
        {
            IEnumerable<Book> BooksByGenre = IBookBL.GetBooksByGenre(id);
            Session["Genre"] = IBookBL.GetGenreByGenreID(id);
            return View(BooksByGenre);
        }

        // Master nav link Language Filter
        public ActionResult Languages(int id)
        {
            IEnumerable<Book> BooksByLanguage = IBookBL.GetBooksByLanguage(id);
            Session["Language"] = IBookBL.GetLanguageByLanguageID(id);
            return View(BooksByLanguage);
        }
        //Remove the Language
        public ActionResult DeleteLanguage(int id)
        {
            ViewBag.Message = IBookBL.DeleteLanguage(id);
            return RedirectToAction("ManageBookLanguage");
        }

        // Remove the Genre
        public ActionResult DeleteGenre(int id)
        {
            ViewBag.Message =  IBookBL.DeleteGenre(id);
            return RedirectToAction("ManageBookCategory");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
       // [AdminAuthorizationFilter]
        [Authorize(Roles = "Admin")]
        public ActionResult AddGenre(FormCollection fc)
        {
            Genre Genre = new Genre();
            Genre.GenreName = fc[1];

            ViewBag.Alert = IBookBL.AddGenre(Genre);

            if (ViewBag.Alert==null)
            {
                ViewBag.Message = "Added Successfully";
                ViewBag.Alert = null;
            }
            return RedirectToAction("ManageBookCategory");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public ActionResult AddLanguage(FormCollection fc)
        {
            Language language = new Language();
            language.LanguageName = fc[1];
            ViewBag.Alert = IBookBL.AddLanguage(language);
            if(ViewBag.Alert == null)
            {
                ViewBag.Message = "Added Successfully";
                ViewBag.Alert = null;
            }
            return RedirectToAction("ManageBookLanguage");
        }

        // Get Meathod for Add new Book
        [HttpGet]
       // [SellerAuthorizationFilter]
       [Authorize(Roles ="Seller")]
        public ActionResult Create()
        {
            ViewBag.Genres = new SelectList(IBookBL.GetAllGenres(),"GenreID","GenreName");
            ViewBag.Languages = new SelectList(IBookBL.GetAllLanguages(), "LanguageID", "LanguageName");
            return View();
        }


        // Post Meathod for Add new Book
        [HttpPost]
       // [SellerAuthorizationFilter]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Seller")]
        public ActionResult Create(AddBookFormVM book)
        {
            ViewBag.Genres = new SelectList(IBookBL.GetAllGenres(), "GenreID", "GenreName");
            ViewBag.Languages = new SelectList(IBookBL.GetAllLanguages(), "LanguageID", "LanguageName");

            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<AddBookFormVM,Book>(); });
                IMapper iMapper = config.CreateMapper();
                Book _book = iMapper.Map<AddBookFormVM, Book>(book);
                _book.UserID = UserController.CurrentUser.UserID;
              

                ViewBag.Alert =  IBookBL.Add(_book);

                if (ViewBag.Alert == null)
                {
                   ViewBag.Message = "Added Successfully";
                    ViewBag.Alert = null;
                }

            }
            return View(book);
        }

        // Edit the Details of the Existing Book
       // [SellerAuthorizationFilter]
        [HttpGet]
        [Authorize(Roles ="Seller")]
        public ActionResult Edit(int id)
        {
            Book book = IBookBL.GetBookByID(id);
            ViewBag.Genres = new SelectList(IBookBL.GetAllGenres(), "GenreID", "GenreName");
            ViewBag.Languages = new SelectList(IBookBL.GetAllLanguages(), "LanguageID", "LanguageName");

            var config = new MapperConfiguration(cfg => { cfg.CreateMap<Book,EditBookFormVM>(); });
            IMapper iMapper = config.CreateMapper();
            EditBookFormVM _book = iMapper.Map<Book,EditBookFormVM>(book);

           

            return View(_book);
        }

        // Delete the Existing Book
        [HttpGet]
        //[SellerAuthorizationFilter]
        [Authorize(Roles ="Seller")]
        public ActionResult Delete(int id)
        {
            IBookBL.Delete(id);
            ViewBag.Message = "Deleted Successfully";
            return RedirectToAction("Index");
        }

        // Post Meathod for Update the Edited Book Details
        [HttpPost]
       // [SellerAuthorizationFilter]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Seller")]
        public ActionResult Update(EditBookFormVM book)
        {
            ViewBag.Genres = new SelectList(IBookBL.GetAllGenres(), "GenreID", "GenreName");
            ViewBag.Languages = new SelectList(IBookBL.GetAllLanguages(), "LanguageID", "LanguageName");

            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<EditBookFormVM, Book>(); });
                IMapper iMapper = config.CreateMapper();
                Book _book = iMapper.Map<EditBookFormVM, Book>(book);

                ViewBag.Alert = IBookBL.Edit(_book);
                if (ViewBag.Alert == null)
                {
                    ViewBag.Message = "Updated Successfully";
                    ViewBag.Alert = null;
                }

                return RedirectToAction("Index");
            }
            return View("Edit", book);
        }
    }
}