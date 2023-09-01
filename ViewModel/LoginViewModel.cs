using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockControll.ViewModel
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "O campo nome é obrigatório")]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo senha é obrigatório"), DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }
    }

    public class RegsiterViewModel
    {
        [Required(ErrorMessage = "Obrigatório")]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        [Display(Name = "Posição")]
        public string Roles { get; set; }

        [Required(ErrorMessage = "Obrigatório"), DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Obrigatório"), DataType(DataType.Password)]
        [Display(Name = "Confirmação de senha")]
        public string ComfirmPassword { get; set; }

        public List<string> SelectedRoles { get; set; }
    }
}
