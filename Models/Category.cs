using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StockControll.Models
{
    public class Category
    {
        [Display(Name = "Identificador unico")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nome do conjunto")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Coloração dos produtos")]
        public string Color { get; set; }

        [Required]
        [Display(Name = "Modelo do solado")]
        public string Sole { get; set; }

        [Required]
        [Display(Name = "Descrição do modelo")]
        public string Description { get; set; }

        [NotMapped]
        public virtual List<Shoe> Shoes { get; set; }

        [NotMapped]
        public virtual Dictionary<int, int> ShoesBySize { get; set; }
    }
}