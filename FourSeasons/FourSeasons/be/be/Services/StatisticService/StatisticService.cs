using be.Models;
using be.Repositories.StatisticRepository;
using Microsoft.EntityFrameworkCore;

namespace be.Services
{
    public class StatisticService : IStatisticService
    {
        public IStatisticRepository _statisticRepo;

        public StatisticService(IStatisticRepository statisticRepository)
        {
            _statisticRepo = statisticRepository;
        }

        public double GetTotalRevenue(int year)
        {
            
            return _statisticRepo.GetTotalRevenue(year);
        }

        public int GetActiveUser(int year)
        {
            return _statisticRepo.GetActiveUser(year);
        }

        public int GetTotalBooking(int year)
        {
            return _statisticRepo.GetTotalBooking(year);
        }

        public List<CollumChart> GetColumnChart(int year)
        {
            return _statisticRepo.GetColumnChart(year);
        }

        public List<BarChart> GetBarChart(int year)
        {
            return _statisticRepo.GetBarChart(year);
        }
    }
}
