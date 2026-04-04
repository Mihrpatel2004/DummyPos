using System.Collections.Generic;

namespace DummyPos.Models
{
    public class DashboardViewModel
    {
        // Top KPI Cards
        public decimal TodayRevenue { get; set; }
        public int TodayOrders { get; set; }
        public double AverageRating { get; set; }
        public int ActiveTables { get; set; }

        // Chart Data
        public List<ChartData> SalesByBranch { get; set; } = new List<ChartData>();
        public List<ChartData> TopSellingItems { get; set; } = new List<ChartData>();
    }

    public class ChartData
    {
        public string Label { get; set; }
        public decimal Value { get; set; }
    }
}