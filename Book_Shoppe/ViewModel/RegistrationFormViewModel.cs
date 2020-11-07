﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Book_Shoppe.Models
{
    public class RegistrationFormViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string MailID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public int RoleID { get; set; }
    }
}