using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StockControll.Models
{
    public class Tag
    {
        [Key]
        public string HashBarCode { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int FootSize { get; set; }

        [Required]
        public string FootSizeId { get; set; }
    }
}