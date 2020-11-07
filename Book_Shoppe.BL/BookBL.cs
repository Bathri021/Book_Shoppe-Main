using Book_Shoppe.DAL;
using Book_Shoppe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Shoppe.BL
{
    public interface IBookBL
    {
        IEnumerable<Book> GetBooks();
        string GetGenreByGenreID(int id);
        string GetLanguageByLanguageID(int id);
        string AddGenre(Genre genre);
        string AddLanguage(Language language);
        string DeleteGenre(int id);
        string DeleteLanguage(int id);
        IEnumerable<Book> GetBooksByGenre(int id);
        IEnumerable<Book> GetBooksByLanguage(int id);
        IEnumerable<Book> SearchResult(string SearchValue);
        IEnumerable<Genre> GetAllGenres();
        IEnumerable<Language> GetAllLanguages();
        IEnumerable<Book> GetUserBooks(int userID);
        IEnumerable<Book> GetUserOrdredBooks(int userID);
        string Add(Book book);
        string Edit(Book book);
        Book GetBookByID(int id);
        bool Delete(int id);
        Book GetBookDetails(int BookID);
    }
    public class BookBL : IBookBL
    {
        IBookRepositary IBookRepos = new BookRepositary();

        public IEnumerable<Book> GetBooks()
        {
            IEnumerable<Book> Books = IBookRepos.GetAllBooks();
            return Books;
        }

        // Static Meathod To Get the Genres for the Master page Nav
        public static IEnumerable<Genre> GetGenres()
        {
            BookRepositary IBookRepos = new BookRepositary();
            IEnumerable<Genre> Generes = IBookRepos.GetAllGenres();
            return Generes;
        }

        // Static Meathod To Get the Language for the Master page Nav
        public static IEnumerable<Language> GetLanguages()
        {
            BookRepositary IBookRepos = new BookRepositary();
            IEnumerable<Language> languages = IBookRepos.GetAllLanguages();
            return languages;
        }

        public string GetGenreByGenreID(int id)
        {
            string GenreName = IBookRepos.GetGenreByGenreID(id);
            return GenreName;
        }

        public string GetLanguageByLanguageID(int id)
        {
            string LanguageName = IBookRepos.GetLanguageByLanguageID(id);
            return LanguageName;
        }

        public string AddGenre(Genre genre)
        {
            // Check the Genre is already exists in the list
            IEnumerable<Genre> genres = IBookRepos.GetAllGenres();
            bool duplications = false;
            foreach (Genre item in genres)
            {
                if (item.GenreName==genre.GenreName)
                {
                    duplications = true;
                }
            }
            if (!duplications)
                return IBookRepos.AddGenre(genre);
            return "Duplication Not Allowed In Genre!";
        }

        public string AddLanguage(Language language)
        {
            // Check the Language is already exists in the list
            IEnumerable<Language> languages = IBookRepos.GetAllLanguages();
            bool duplications = false;
            foreach (Language item in languages)
            {
                if(item.LanguageName == language.LanguageName)
                {
                    duplications = true;
                }
            }
            if (!duplications)
                return IBookRepos.AddLanguage(language);
            return "Duplications Not Allowed In Language!";            
        }

        public string DeleteGenre(int id)
        {
            return IBookRepos.DeleteGenre(id);
        }

        public string DeleteLanguage(int id)
        {
            return IBookRepos.DeleteLanguage(id);
        }

        public IEnumerable<Book> GetBooksByGenre(int id)
        {
            IEnumerable<Book> BooksByGenre = IBookRepos.GetBooksByGenre(id);
            return BooksByGenre;
        }

        public IEnumerable<Book> GetBooksByLanguage(int id)
        {
            return IBookRepos.GetBooksByLanguage(id);
        }

        public IEnumerable<Book> SearchResult(string SearchValue)
        {
            IEnumerable<Book> SearchedBooks = IBookRepos.SearchResult(SearchValue);
            return SearchedBooks;
        }
        public IEnumerable<Genre> GetAllGenres()
        {
            IEnumerable<Genre> Generes = IBookRepos.GetAllGenres();
            return Generes;
        }

        public IEnumerable<Language> GetAllLanguages()
        {
            return IBookRepos.GetAllLanguages();
        }

        public IEnumerable<Book> GetUserBooks(int userID)
        {
            IEnumerable<Book> Books = IBookRepos.GetBookByUserID(userID);
            return Books;
        }

        public IEnumerable<Book> GetUserOrdredBooks(int userID)
        {
            IEnumerable<Book> Books = IBookRepos.GetOrderedBookBySellerID(userID);
            return Books;
        }

        public string Add(Book book)
        {  
            // Check the Book Title of the current requested Book from Existing Books
            IEnumerable<Book> Books = IBookRepos.GetAllBooks();
            bool duplications = false;
            foreach (var item in Books)
            {
                if (item.Title == book.Title)
                {
                    duplications = true;
                }
            }
            if (!duplications)
                return IBookRepos.Add(book);
            else
                return "Duplication Not Allowed In Book Title!";
           
        }

        public string Edit(Book book)
        {
            // Check the Book Title of the current requested Book from Existing Books
            IEnumerable<Book> Books = IBookRepos.GetAllBooks();
            short count = 0;
            foreach (var item in Books)
            {
                if (item.Title == book.Title)
                {
                    count++;
                }
            }
            if (count<=1)
                return IBookRepos.Edit(book);
            else
                return "Duplication Not Allowed In Book Title!";
        }

     
        public Book GetBookByID(int id)
        {
            return IBookRepos.GetBookByID(id);
        }

        public bool Delete(int id)
        {
            IBookRepos.Delete(id);
            return true;
        }

        public Book GetBookDetails(int BookID)
        {
            return IBookRepos.GetBookDetails(BookID);
        }

    }
}
