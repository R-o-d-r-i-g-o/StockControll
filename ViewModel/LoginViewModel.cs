using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockControll.ViewModel
{
    public class UserViewModel
    {
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo nome é obrigatório")]
        public string Name { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "O campo senha é obrigatório"), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
