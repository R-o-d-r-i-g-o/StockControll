using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static StockControll.Commons.Enums;

namespace StockControll.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo email é obrigatório")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória"), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "O cargo do usuário é obrigatório")]
        public UserType UserType { get; set; }

        [Column(TypeName = "VARCHAR"), StringLength(11)]
        public string CPF { get; set; }

        [Required(ErrorMessage = "A data de criação deve ser registrada")]
        public DateTime CreatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
