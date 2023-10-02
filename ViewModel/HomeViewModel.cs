using System;
using System.Collections.Generic;
using System.Linq;
using StockControll.Models;

namespace StockControll.ViewModel
{
    public class HomeViewModel
    {
        public FilterViewModel Filters { get; set; }
        public List<SoldItens> SaleDetails { get; set; }
        public int ShoesRegistred { get; set; }
        public int ModelsRegistred { get; set; }

        public ChartData GetSalesByDay()
        {
            var startDate = Filters.StartDate;
            var endDate = Filters.EndDate;

            // Lista de todos os dias do mês para o período desejado
            var dateRange = Enumerable.Range(0, (int)(endDate - startDate).TotalDays + 1)
                .Select(offset => startDate.AddDays(offset))
                .ToList();

            // Dicionário com datas como chave e total de vendas em R$
            var groupedSale = SaleDetails
                .GroupBy(s => s.Sale.CreatedAt.Date)
                .ToDictionary(g => g.Key, g => g.Sum(s => s.Shoe.Price));

            var labels = dateRange.Select(date => date.ToString("dd/MM/yyyy")).ToList();
            var prices = dateRange.Select(date => groupedSale.ContainsKey(date) ? groupedSale[date] : 0).ToList();

            return new ChartData
            {
                DayOfMonth = labels,
                GrupedPrices = prices,
            };
        }

        public decimal AverageTicket()
        {
            var totalAmount = SaleDetails.Sum(s => s.Shoe.Price);
            var totalCount = SaleDetails.Select(_ => 1).Count();

            if (totalCount <= 0)
                totalCount = 1;

            return totalAmount / totalCount;
        }
    }

    public class ChartData
    {
        public List<decimal> GrupedPrices { get; set; }
        public List<string> DayOfMonth { get; set; }
    }

    public class SoldItens
    {
        public Sale Sale { get; set; }
        public Shoe Shoe { get; set; }
    }
}