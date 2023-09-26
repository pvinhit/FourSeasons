using be.Models;

namespace be.Repositories.StatisticRepository
{
    public interface IStatisticRepository
    {
        public double GetTotalRevenue(int year);
        public int GetActiveUser(int year);
        public int GetTotalBooking(int year);
        public List<CollumChart> GetColumnChart(int year);
        public List<BarChart> GetBarChart(int year);
    }
}
