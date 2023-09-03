using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static StockControll.Commons.Enums;

namespace StockControll.Models
{
    public class Log
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O usuário deve estar vinculado ao registro")]
        public virtual User User { get; set; }

        [Required(ErrorMessage = "O campo tipo de atividade é obrigatório")]
        public ActivityType ActivityType { get; set; }

        [Required(ErrorMessage = "O campo email é obrigatório")]
        public string Message { get; set; }

    }
}