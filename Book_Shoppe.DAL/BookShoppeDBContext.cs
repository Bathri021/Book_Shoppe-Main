using Book_Shoppe.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Shoppe.DAL
{
    public class BookShoppeDBContext : DbContext
    {
        public BookShoppeDBContext():base("name=DBContext")
        {
           
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<WishList> WishList { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartBook> CartBooks { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<User>().HasIndex(u => u.MailID).IsUnique();
            builder.Entity<User>().HasIndex(u => u.UserName).IsUnique();
            builder.Entity<Book>().HasIndex(u => u.Title).IsUnique();
            builder.Entity<Genre>().HasIndex(u => u.GenreName).IsUnique();
            builder.Entity<Language>().HasIndex(u => u.LanguageName).IsUnique();
            builder.Entity<User>().MapToStoredProcedures();

            builder.Entity<CartBook>().HasKey(cb => cb.CartBookID);
        }
    }
}
