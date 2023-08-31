﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockControll.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Obrigatório"), DataType(DataType.Password)]
        public string Password { get; set; }

        public UserType UserType { get; set; }

        [Column(TypeName = "VARCHAR"), StringLength(250)]
        public string PasswordResetToken { get; set; }

        [Column(TypeName = "VARCHAR"), StringLength(20)]
        public string CPF { get; set; }
    }

    public enum UserType
    { 
        notSetted,
        normalOne,
        Admin
    }
}