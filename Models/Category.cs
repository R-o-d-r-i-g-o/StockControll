using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

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
        [Display(Name = "Data de criação")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Display(Name = "Descrição do modelo")]
        public string Description { get; set; }

        [NotMapped]
        [JsonIgnore]
        public virtual List<Shoe> Shoes { get; set; }

        [NotMapped]
        [JsonIgnore]
        public virtual Dictionary<int, int> ShoesBySize { get; set; }
    }
}