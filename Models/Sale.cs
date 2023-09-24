using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StockControll.Models
{
    public class Sale
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Shoe")]
        [Display(Name = "Produto referido")]
        public string Shoe_BarCodeHash { get; set; }

        [Key]
        [Column(Order = 2)]
        [ForeignKey("Seller")]
        [Display(Name = "Usuário referido")]
        public int Seller_Id { get; set; }

        [Required]
        [Display(Name = "Data de remoção do estoque")]
        public DateTime CreatedAt { get; set; }

        [NotMapped]
        [Display(Name = "Informativo de venda")]
        public string Note { get; set; }

        public virtual Shoe Shoe { get; set; }
        public virtual User Seller { get; set; }
    }
}