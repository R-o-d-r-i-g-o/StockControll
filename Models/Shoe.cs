using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockControll.Models
{
    public class Shoe
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O sapato deve estar vinculado a alguma etiqueta")]
        public virtual Tag Tag { get; set; }

        //[Required]
        //public int LastChange { get; set; }
    }
}