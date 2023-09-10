using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockControll.Models
{
    public class Shoe
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public uint LastChange { get; set; }

        [Required]
        public uint Price { get; set; }

        [Required]
        public DateTime SoldAt { get; set; }
    }
}