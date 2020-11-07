using Book_Shoppe.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Shoppe.DAL
{
    public interface IUserRepositary
    {
       IEnumerable<User> GetUsers();
       IEnumerable<Role> GetRoles();
       string AddUser(User user);
       User ValidateLogIn(string userName, string password);
       User GetUserByID(int userID);
       string EditUser(User user);
       bool Delete(int id);
       string AddToWishList(int userID,int bookID);
       IEnumerable<Book> GetUserWishlist(int id);
       bool CheckBookInWishList(int userID, int bookID);
       void RemoveBookFormWishlist(int id);
       bool CheckBookInUserCart(int userID, int bookID);
       int AddToCart(int userID, int bookID);
       int GetCartRate(int cartID);
       IEnumerable<Book> GetUserCartDetails(int id);
       void RemoveBookFormUserCart(int id);
       bool AddShipment(Shipment shipment);
       bool PlaceOrder(Shipment shipment);
       bool CheckCartInOrders(int id);
       IEnumerable<Book> GetUserOrderDetails(int id);
    }

    public class UserRepositary : IUserRepositary
    {
        public IEnumerable<User> GetUsers()
        {
            BookShoppeDBContext userDBContext = new BookShoppeDBContext();
            return userDBContext.Users.Include("Role").Where(m=>m.RoleID<=2).ToList();
        }

        public IEnumerable<Role> GetRoles()
        {
            BookShoppeDBContext RoleContext = new BookShoppeDBContext();
            return RoleContext.Roles.Where(m => m.RoleName=="Seller" || m.RoleName=="Customer").ToList();
        }
       
        public string AddUser(User user)
        {
            BookShoppeDBContext _Context = new BookShoppeDBContext();
            _Context.Users.Add(user);
    
                _Context.SaveChanges();
          
            return null;
        }

        public User ValidateLogIn(string userName,string password)
        {
            User _user=null;
            BookShoppeDBContext _Context = new BookShoppeDBContext();
            _user = _Context.Users.Include("Role").Where(u=> u.UserName==userName && u.Password==password).SingleOrDefault();
            return _user;
        }
        public User GetUserByID(int userID)
        {
            BookShoppeDBContext _context = new BookShoppeDBContext();
            return _context.Users.Include("Role").SingleOrDefault(ID => ID.UserID == userID);
        }
        public string EditUser(User user)
        {
            BookShoppeDBContext _context = new BookShoppeDBContext();
            _context.Entry(user).State = System.Data.Entity.EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException.InnerException.Message != null)
                {
                    return "The User Name should not be duplicated";
                }
                else
                {
                    return "Please fill out the form correctly and sumbit your values";
                }
            }
            return null;
        }
        public bool Delete(int id)
        {
            using (var Context = new BookShoppeDBContext())
            {
                User user = Context.Users.Where(ID => ID.UserID == id).FirstOrDefault();
                Context.Users.Remove(user);
                Context.SaveChanges();
                return true;
            }
        }

        public string AddToWishList(int userID, int bookID)
        {
            using(BookShoppeDBContext _context = new BookShoppeDBContext())
            {
                WishList wishlist = new WishList()
                {
                    UserID = userID,
                    BookID = bookID
                };
                _context.WishList.Add(wishlist);
                _context.SaveChanges();
                return null;
            }
        }

        public void RemoveBookFormWishlist(int id)
        {
            using(BookShoppeDBContext _context = new BookShoppeDBContext())
            {
                WishList wishlist = _context.WishList.Where(w => w.BookID == id).SingleOrDefault();
                _context.WishList.Remove(wishlist);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Book> GetUserWishlist(int id)
        {
            List<Book> books = new List<Book>();
            using(BookShoppeDBContext _context = new BookShoppeDBContext())
            {
              List<WishList> wishlist =  _context.WishList.Where(ID => ID.UserID == id).ToList();

                foreach (var item in wishlist)
                {
                    Book book = _context.Books.Where(ID => ID.BookID == item.BookID).SingleOrDefault();
                    books.Add(book);
                }
                return books;
            }
        }

        public bool CheckBookInWishList(int userID, int bookID)
        {
            using(BookShoppeDBContext _context = new BookShoppeDBContext())
            {
              WishList wishlist =  _context.WishList.Where(ID => ID.UserID == userID && ID.BookID == bookID).SingleOrDefault();
                if (wishlist==null)
                    return false;
                return true;
            }
        }

        public bool CheckBookInUserCart(int userID, int bookID)
        {
            using (BookShoppeDBContext _context = new BookShoppeDBContext())
            {
                Cart cart = GetCartNotInOrder(userID);
                //Order order = _context.Orders.Where(o => o.CartID == cart.CartID).SingleOrDefault();
                if (cart == null)
                {
                    return false; // Book is not exist in User Cart
                }
                else
                {
                   int cartID = cart.CartID;
                   CartBook cartBook = _context.CartBooks.Where(cb => cb.CartID == cartID && cb.BookID == bookID).SingleOrDefault();
                    if(cartBook==null)
                        return false; // Book is not exist in User Cart
                    return true; // Book is already added in User Cart 
                }
            }
        }

        // Such a complex coding - to check the cart exist in the orders table and which cart is not in the order then the book is added in the cart
        // if the user dont have any cart then it create a new cart and add the book
        // if the user has only one cart in orders then it creates new cart and add the book in the cart
        public int AddToCart(int userID, int bookID)
        {
            using(BookShoppeDBContext _context = new BookShoppeDBContext())
            {
                List<Cart> user_carts = _context.Carts.Where(c => c.UserID == userID).ToList();
                Cart _cart = new Cart();

                int no_of_carts = user_carts.Count();

                if (no_of_carts == 2) // When User Has Two Carts Probably One In Orders Another One For New Add To Carts
                {
                    Cart cartOne = user_carts.ElementAt(0);
                    Cart cartTwo = user_carts.ElementAt(1);

                    if (cartOne.IsOrdered == false) // Check Wheather The First One Is Not In Orders To Add The New Book
                    {
                        CartBook cartbooks = new CartBook()
                        {
                            CartID = cartOne.CartID,
                            BookID = bookID,
                        };

                        // Get Book Into Object And Add The Book Rate Into The Cart Rate
                        Book book = _context.Books.Where(b => b.BookID == bookID).SingleOrDefault();
                        cartOne.CartRate = cartOne.CartRate + book.Price;

                        _context.CartBooks.Add(cartbooks);
                        _context.SaveChanges();
                        return cartOne.CartID;
                    }
                    else if (cartTwo.IsOrdered == false) //Or Else Check Wheather The First Ssecond Is Not In Orders To Add The New Book
                    {
                        CartBook cartbooks = new CartBook()
                        {
                            CartID = cartTwo.CartID,
                            BookID = bookID,
                        };

                        // Get Book Into Object And Add The Book Rate Into The Cart Rate
                        Book book = _context.Books.Where(b => b.BookID == bookID).SingleOrDefault();
                        cartOne.CartRate = cartOne.CartRate + book.Price;

                        _context.CartBooks.Add(cartbooks);
                        _context.SaveChanges();
                        return cartTwo.CartID;
                    }
                }
                else if (no_of_carts == 1) // When The User Has One Cart That May Be In Orders Or In Carts To Add New Books 
                {
                    Cart cart = user_carts.ElementAt(0);

                    if (cart.IsOrdered == false) // Check If the Cart Is Not Added Into the Orders
                    {
                        CartBook cartbooks = new CartBook()
                        {
                            CartID = cart.CartID,
                            BookID = bookID,
                        };

                        // Get Book Into Object And Add The Book Rate Into The Cart Rate
                        Book book = _context.Books.Where(b => b.BookID == bookID).SingleOrDefault();
                        cart.CartRate = cart.CartRate + book.Price;

                        _context.CartBooks.Add(cartbooks);
                        _context.SaveChanges();
                        return cart.CartID;
                    }
                    else if (cart.IsOrdered != false)// Check If The Cart Is Already Added In To The Orders
                    {
                        // Create A New Cart For The User
                        Cart new_cart = new Cart()
                        {
                            UserID = userID,
                        };
                        _context.Carts.Add(new_cart);
                        _context.SaveChanges();
                        _cart = GetCartNotInOrder(userID);

                        if (_cart != null)
                        {
                            CartBook cartbooks = new CartBook()
                            {
                                CartID = _cart.CartID,
                                BookID = bookID,
                            };

                            // Get Book Into Object And Add The Book Rate Into The Cart Rate
                            Book book = _context.Books.Where(b => b.BookID == bookID).SingleOrDefault();
                            _cart.CartRate = _cart.CartRate + book.Price;

                            _context.CartBooks.Add(cartbooks);
                            _context.SaveChanges();
                            return _cart.CartID;
                        }
                    }
                }
                else if (no_of_carts==0) // Wheather The User Has No Carts
                {
                        // Create A New Cart For The User
                        Cart cart = new Cart()
                        {
                            UserID = userID,
                        };
                        _context.Carts.Add(cart);
                        _context.SaveChanges();

                        cart = GetCartNotInOrder(userID);

                    using (var dbContextTransaction=_context.Database.BeginTransaction())
                    {
                        CartBook cartbook = new CartBook()
                        {
                            CartID = cart.CartID,
                            BookID = bookID,
                        };
                        _context.CartBooks.Add(cartbook);

                        // Get Book Into Object And Add The Book Rate Into The Cart Rate
                        Book book = _context.Books.Where(b => b.BookID == bookID).SingleOrDefault();
                        cart.CartRate = book.Price + cart.CartRate;

                        _context.SaveChanges();
                        dbContextTransaction.Commit();

                        return cart.CartID;
                    }
                   
                }

                //foreach (Cart cart in user_carts)
                //{
                //    // Order order = _context.Orders.Where(o => o.CartID == cart.CartID).SingleOrDefault();


                //    if (cart.IsOrdered == false) // The Current Cart Is Not In The Orders Table
                //    {
                //        CartBook cartbooks = new CartBook()
                //        {
                //            CartID = cart.CartID,
                //            BookID = bookID,
                //        };

                //        // Get Book Into Object And Add The Book Rate Into The Cart Rate
                //        Book book = _context.Books.Where(b => b.BookID == bookID).SingleOrDefault();
                //        cart.CartRate = book.Price + cart.CartRate;

                //        _context.CartBooks.Add(cartbooks);
                //        _context.SaveChanges();
                //        return cart.CartID;
                //    }
                //    else if (cart.IsOrdered != false) // The Cart Is Already Added In The Orders
                //    {
                //        Cart new_cart = new Cart()
                //        {
                //            UserID = userID,
                //        };
                //        _context.Carts.Add(new_cart);
                //        _context.SaveChanges();
                //         _cart = GetCartNotInOrder(userID);

                //        if (_cart!=null)
                //        {
                //            CartBook cartbooks = new CartBook()
                //            {
                //                CartID = _cart.CartID,
                //                BookID = bookID,
                //            };
                //            _context.CartBooks.Add(cartbooks);
                //            _context.SaveChanges();
                //            return cart.CartID;
                //        }
                //    }
                //}


                //if (user_carts.Count==0)
                //{
                //    Cart cart = new Cart()
                //    {
                //        UserID = userID,
                //    };
                //    _context.Carts.Add(cart);
                //    _context.SaveChanges();
                //    _cart = GetCartNotInOrder(userID);

                //    CartBook cartbook = new CartBook()
                //    {
                //         CartID = _cart.CartID,
                //         BookID = bookID,
                //    };
                //    _context.CartBooks.Add(cartbook);
                //    _context.SaveChanges();
                //    return cart.CartID;
                //}

                return 0;
            }
        }

        // Not written in the interface because it is used to access only with in this class
        private Cart GetCartNotInOrder(int userID)
        {
            List<Cart> User_Carts = new List<Cart>();
            using (BookShoppeDBContext _context = new BookShoppeDBContext())
            {
                User_Carts = _context.Carts.Where(c => c.UserID == userID).ToList();

                foreach(Cart cart in User_Carts)
                {

                    // Order order = _context.Orders.Where(o => o.CartID == cart.CartID).SingleOrDefault();

                    if (cart.IsOrdered == false)
                    {
                        return cart;
                    }
                }
                return null;
            }
        }

        // Not Used In Function
        private Cart CheckCartInOrder(int cartID)
        {
            using (BookShoppeDBContext _context = new BookShoppeDBContext())
            {
                    Cart cart= _context.Carts.Where(c => c.UserID == cartID).SingleOrDefault() ;
               
                    Order order = _context.Orders.Where(o => o.CartID == cartID).SingleOrDefault();

                    if (order == null)
                    {
                        return cart;
                    }
                
                return null;
            }
        }

        public int GetCartRate(int cartID)
        {
            using (BookShoppeDBContext _context = new BookShoppeDBContext())
            {
                int cartRate = 0;
                List<CartBook> cartBooks = _context.CartBooks.Where(cb => cb.CartID == cartID).ToList();

                foreach (var item in cartBooks)
                {
                    Book book = _context.Books.Where(ID => ID.BookID == item.BookID).SingleOrDefault();
                    cartRate = cartRate + book.Price; 
                }
                return cartRate;
            }

        }

        public IEnumerable<Book> GetUserCartDetails(int id)
        {
            List<Book> books = new List<Book>();
            using (BookShoppeDBContext _context = new BookShoppeDBContext())
            {
                try
                {
                    Cart cart = new Cart();
                    cart = GetCartNotInOrder(id);
                    if (cart != null)
                    {
                        int cartID = cart.CartID;

                        List<CartBook> cartBooks = _context.CartBooks.Where(cb => cb.CartID == cartID).ToList();

                        foreach (var item in cartBooks)
                        {
                            Book book = _context.Books.Where(ID => ID.BookID == item.BookID).SingleOrDefault();
                            books.Add(book);
                        }
                     return books;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public void RemoveBookFormUserCart(int id)
        {
            using (BookShoppeDBContext _context = new BookShoppeDBContext())
            {
                CartBook cartBooks = _context.CartBooks.Where(cb => cb.BookID == id).SingleOrDefault();
                _context.CartBooks.Remove(cartBooks);
                _context.SaveChanges();
            }
        }

        public bool AddShipment(Shipment shipment)
        {
            using (BookShoppeDBContext _context = new BookShoppeDBContext())
            {
                Shipment _shipment = _context.Shipments.Where(s => s.Address == shipment.Address && s.UserID == shipment.UserID).FirstOrDefault();

                if (_shipment==null)
                {
                    _context.Shipments.Add(shipment);
                    _context.SaveChanges();
                }
                else if (_shipment.Address != shipment.Address && _shipment.UserID != shipment.UserID)
                {
                   _context.Shipments.Add(shipment);
                   _context.SaveChanges();
                }

                _shipment = _context.Shipments.Where(s => s.Address == shipment.Address && s.UserID == shipment.UserID).SingleOrDefault() ;

                return PlaceOrder(_shipment);
            }
        }

        public bool PlaceOrder(Shipment shipment)
        {
            using (BookShoppeDBContext _context = new BookShoppeDBContext())
            {
                Order order = new Order();
                order.ShipmentID = shipment.ShipmentID;
                Cart cart = _context.Carts.Where(c => c.UserID == shipment.UserID && c.IsOrdered == false).SingleOrDefault();
                order.CartID = cart.CartID;

                _context.Orders.Add(order);
                cart.IsOrdered = true;

                _context.SaveChanges();

                return true;
            }
        }

        public bool CheckCartInOrders(int id)
        {
            using (BookShoppeDBContext _context = new BookShoppeDBContext())
            {
                Cart cart = _context.Carts.Where(c => c.UserID == id).SingleOrDefault();
                Order order = _context.Orders.Where(o => o.CartID == cart.CartID).Single();

                if (order == null)
                    return false; // Return false when the Cart is not in the orders

                return true; // Return true when the cart is available in the orders
            }

        }

        public IEnumerable<Book> GetUserOrderDetails(int id)
        {
            using (BookShoppeDBContext _context = new BookShoppeDBContext())
            {
                try
                {
                    List<Book> UserOrderedBooks = new List<Book>();
                    int cartID = 0;
                    List<Cart> UserCarts = _context.Carts.Where(c => c.UserID == id).ToList();

                    foreach(Cart cart in UserCarts)
                    {
                        Order order = _context.Orders.Where(o => o.CartID == cart.CartID).SingleOrDefault();
                        if(order != null)
                        {
                            cartID = cart.CartID;
                        }
                    }


                    if (cartID != 0)
                    {
                        List<CartBook> cartBooks = _context.CartBooks.Where(cb => cb.CartID == cartID).ToList();

                        foreach (var item in cartBooks)
                        {
                            Book book = _context.Books.Where(ID => ID.BookID == item.BookID).SingleOrDefault();
                            UserOrderedBooks.Add(book);
                        }
                        return UserOrderedBooks;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

        }
    }
}
