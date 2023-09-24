using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockControll.Models
{
    public class Shoe
    {
        [Key]
        [Required]
        [Display(Name = "Código do produto")]
        public string BarCodeHash { get; set; }

        [Required]
        [Display(Name = "Valor unitário")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Tamanho do exemplar")]
        public int Size { get; set; }

        [Required]
        [Display(Name = "Unidade de medida do tamanho")]
        public string SizeId { get; set; }

        [Required]
        [Display(Name = "Data de registro do calçado")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Display(Name = "Dados complementares")]
        public Category Category { get; set; }
    }
}