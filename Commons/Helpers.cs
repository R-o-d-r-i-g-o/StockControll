using System;
using System.ComponentModel;
using System.Linq;
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

        public static bool IsValidCPF(this string cpf)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11)
                return false;

            if (new string(cpf[0], 11) == cpf)
                return false;

            // Calcula o primeiro dígito verificador
            int sum = 0;
            for (int i = 0; i < 9; i++) {
                sum += int.Parse(cpf[i].ToString()) * (10 - i);
            }
            int remainder = sum % 11;
            int digit1 = remainder < 2 ? 0 : 11 - remainder;

            // Calcula o segundo dígito verificador
            sum = 0;
            for (int i = 0; i < 10; i++) {
                sum += int.Parse(cpf[i].ToString()) * (11 - i);
            }
            remainder = sum % 11;
            int digit2 = remainder < 2 ? 0 : 11 - remainder;

            // Verifica se os dígitos verificadores são iguais aos dígitos no CPF
            return digit1 == int.Parse(cpf[9].ToString()) && digit2 == int.Parse(cpf[10].ToString());
        }
    }
}