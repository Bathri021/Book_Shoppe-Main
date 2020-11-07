using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Book_Shoppe.ViewModel
{
    public class GetShipmentDetailsVM
    {
        [Required]
        public int ShipmentID { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public int UserID { get; set; }
    }
}