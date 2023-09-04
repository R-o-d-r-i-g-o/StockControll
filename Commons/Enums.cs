using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockControll.Commons
{
    public static class Enums
    {
        public enum UserType
        {
            [Description("Usuário")]
            normalUser,

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

        public static List<SelectListItem> GetUsertypeOptions(Enums.UserType? userType = null)
        {
            return Enum.GetValues(typeof(Enums.UserType)).Cast<Enums.UserType>().Select(u => new SelectListItem {
                Text = u.GetDescription(),
                Value = u.ToString(),
                Selected = u == userType
            })
            .ToList();
        }
    }
}