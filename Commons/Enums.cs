using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace StockControll.Commons
{
    public class Enums
    {
        public enum UserType
        {
            [Description("Não configurado")]
            notSetted,

            [Description("Usuário")]
            normalOne,

            [Description("Admin.")]
            Admin
        }

        public enum ActivityType
        {
            [Description("Editar recursos")]
            EditItems,

            [Description("Deletar recursos")]
            DeleteItems,

            [Description("Outros")]
            Other,
        }
    }
}