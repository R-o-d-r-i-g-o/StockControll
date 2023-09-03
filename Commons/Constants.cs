using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControll.Commons
{
    public static class Constants
    {
        private const string _SHOWN_PASSWORD = "******";

        public static string HidePassword(this string _) => _SHOWN_PASSWORD;
    }
}