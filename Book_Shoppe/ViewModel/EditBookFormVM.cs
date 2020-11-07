using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Book_Shoppe.Models
{
    public class EditBookFormVM
    {
        public int BookID { get; set; }

        public int UserID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public int GenreID { get; set; }

        [Required]
        public int LanguageID { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public int Price { get; set; }

        [Required]
        public int NoOfPages { get; set; }
    }
}