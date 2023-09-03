using System;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace StockControll.Commons
{
    public static class Helpers
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            if (fieldInfo != null) {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0) {
                    return attributes[0].Description;
                }
            }

            return value.ToString();
        }

        // string formats

        public static bool IsEmailValid(this string email) // format meets "d*****.@aluno.educ.al.gov.br" too.
            => email.Length <= 50 && Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$");

        public static string AddMaskToCnpj(this string cnpj) 
            => Regex.Replace(cnpj, @"(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})", "$1.$2.$3/$4-$5");

        public static string AddMaskToCpf(this string cpf)
            => Regex.Replace(cpf, @"(\d{3})(\d{3})(\d{3})(\d{2})", "$1.$2.$3-$4");

        public static string UnmaskOnlyNumbers(this string value)
            => Regex.Replace(value, @"[^\d]", "");
    }
}