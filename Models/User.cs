using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static StockControll.Commons.Enums;

namespace StockControll.Models
{
    public class User
    {
        [Display(Name = "Identificador unico")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo nome é obrigatório")]
        public string Name { get; set; }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "O campo email é obrigatório")]
        public string Email { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "A senha é obrigatória"), DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Cargo")]
        [Required(ErrorMessage = "O cargo do usuário é obrigatório")]
        public UserType UserType { get; set; }

        [Display(Name = "CPF (sem mascara)")]
        [Column(TypeName = "VARCHAR"), StringLength(11)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve conter 11 dígitos numéricos.")]
        public string CPF { get; set; }

        [Display(Name = "Data de criação")]
        [Required(ErrorMessage = "A data de criação deve ser registrada")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Data de encerramento")]
        public DateTime? DeletedAt { get; set; }
    }
}
