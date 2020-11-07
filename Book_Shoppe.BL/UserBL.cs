using Book_Shoppe.DAL;
using Book_Shoppe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Shoppe.BL
{
    public interface IUserBL
    {
        IEnumerable<User> GetUsers();
        IEnumerable<Role> GetRoles();
        string AddUser(User user);
        User LogIn(string userName, string password);
        User GetUserByID(int id);
        string EditUser(User user);
        bool Delete(int id);
        string AddToWishList(int userID, int bookID);
        void RemoveBookFormWishlist(int id);
        IEnumerable<Book> GetUserWishlist(int id);
        IEnumerable<Book> GetUserCartDetails(int id);
        string AddToCart(int userID, int bookID);
        void RemoveBookFormUserCart(int id);
        bool AddShipment(Shipment shipment);
        IEnumerable<Book> GetUserOrderDetails(int id);
    }

    public class UserBL : IUserBL
    {
        IUserRepositary IUserRepos = new UserRepositary();

        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> Users = IUserRepos.GetUsers();
            return Users;
        }

        public IEnumerable<Role> GetRoles()
        {
            IEnumerable<Role> Roles = IUserRepos.GetRoles();
            return Roles;
        }

        public string AddUser(User user)
        {
            // Check the User Name of the current requested User from Existing Users
            IEnumerable<User> Users = IUserRepos.GetUsers();
            bool duplications=false;
            foreach (var item in Users)
            {
               if( item.UserName == user.UserName || item.MailID == user.MailID)
                {
                    duplications = true;
                } 
            }
            if (!duplications)
                return IUserRepos.AddUser(user);
            else
                return "Duplication Not Allowed In UserName and Email!";
        }

        public User LogIn(string userName,string password)
        {
            return IUserRepos.ValidateLogIn(userName, password);
        }

        public User GetUserByID(int id)
        {
            return IUserRepos.GetUserByID(id);
        }

        public string EditUser(User user)
        {
            return IUserRepos.EditUser(user);
        }

        public bool Delete(int id)
        {
            return IUserRepos.Delete(id);
        }

        public string AddToWishList(int userID, int bookID)
        {
            // Check wheather the Book is already added in the Wishlist
            if (IUserRepos.CheckBookInWishList(userID, bookID))
                return "Book already added in the wishlist";
            return IUserRepos.AddToWishList(userID, bookID);
        }

        public void RemoveBookFormWishlist(int id)
        {
            IUserRepos.RemoveBookFormWishlist(id);
        }

        public IEnumerable<Book> GetUserWishlist(int id)
        {
            return IUserRepos.GetUserWishlist(id);
        }

        public IEnumerable<Book> GetUserCartDetails(int id)
        {
            //// Check wheather the cart is already added in the orders table
            //if (IUserRepos.CheckCartInOrders(id))
            //    return null; // If the cart is added then the cart details should not be shown

            return IUserRepos.GetUserCartDetails(id);
        }

        public string AddToCart(int userID,int bookID)
        {
            if (IUserRepos.CheckBookInUserCart(userID, bookID))
                return "Book already added in the Cart";

            int userCartID = IUserRepos.AddToCart(userID, bookID);

            if (userCartID != 0)
            {
              int cartRate = IUserRepos.GetCartRate(userCartID);
            }
            return null;
        }

        public void RemoveBookFormUserCart(int id)
        {
           IUserRepos.RemoveBookFormUserCart(id);
        }

        public bool AddShipment(Shipment shipment)
        {
           return IUserRepos.AddShipment(shipment);
        }

        public IEnumerable<Book> GetUserOrderDetails(int id)
        {
            return IUserRepos.GetUserOrderDetails(id);
        }
    }
}
