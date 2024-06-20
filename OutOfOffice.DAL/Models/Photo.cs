﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOffice.DAL.Models
{
    public class Photo
    {
        public int Id { get; set; }

        [Required]
        public byte[] Base64Data { get; set; } = null!;
    }
}