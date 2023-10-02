using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControll.Commons
{
    public static class Constants
    {
        private const string _SHOWN_PASSWORD = "******";

        private static int[] _PAGINATION = new int[] { 10, 20, 30, 50, 100 };

        private static int[] _FOOT_SIZES = new int[] { 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52 };

        // Exported methods
        public static string HidePassword(this string _) => _SHOWN_PASSWORD;
        public static int[] GetPossibleFootSizes() => _FOOT_SIZES;
        public static int[] GetPaginateOptions() => _PAGINATION;
    }
}