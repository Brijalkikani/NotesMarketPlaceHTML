﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Notesmarketplace1.Models
{
    public class AddCategory
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string Description { get; set; }

    }
}