using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Book_Shoppe.Entity
{
    [Table("User")]
    public class User
    {
        [Key]
        [Column("UserID")]
        public int UserID { get; set; }

        [Column("RoleID")]
        [Required]
        public int RoleID { get; set; }

        [ForeignKey("RoleID")]
        public Role Role { get; set; }

       
        [Required]
        [MaxLength(26)]
        public string Name { get; set; }

       
        [Required]
        [MaxLength(26)]
        public string UserName { get; set; }


        [Required]
        [MaxLength(64)]
        public string MailID { get; set; }

 
        [Required]
        [MaxLength(12)]
        public string Password { get; set; }


        
        public List<WishList> WishLists { get; set; }
    }


    public class Role
    {
        [Key]
        public int RoleID { get; set; }

        [Required]
        [MaxLength(8)]
        public string RoleName { get; set; }
    }

    public class WishList
    {
        [Key]
        public int WishListID { get; set; }

        [Required]
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }

        [Required]
        public int BookID { get; set; }
        [ForeignKey("BookID")]
        public Book Book { get; set; }
    }

    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required]
        public int CartID { get; set; }
        [ForeignKey("CartID")]
        public Cart Cart { get; set; }
       
        [Required]
        public int ShipmentID { get; set; }
        [ForeignKey("ShipmentID")]
        public Shipment Shipment { get; set; }

    }

    public class Shipment
    {
        [Key]
        public int ShipmentID { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }
    }

    public class Cart
    {
        public int CartID { get; set; }

        [Required]
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }

        public int CartRate { get; set; }

        public bool IsOrdered { get; set; }

        public List<CartBook> CartBooks { get; set; }
    }

    public class CartBook
    {
        [Key]
        public int CartBookID { get; set; }

        [Required]
        public int CartID { get; set; }
        [ForeignKey("CartID")]
        public Cart Cart { get; set; }

        [Required]
        public int BookID { get; set; }
        [ForeignKey("BookID")]
        public Book Book { get; set; }
    }
    //public class OrdersShipment
    //{
    //    public int OrdersShipmentID { get; set; }

    //    public int OrderID { get; set; }
    //    public Order Order { get; set; }

    //    public int ShipmentID { get; set; }
    //    public Shipment Shipment { get; set; }
    //}
}
