using be.Models;

namespace be.Repositories.StatisticRepository
{
    public class StatisticRepository : IStatisticRepository
    {
        private readonly DbFourSeasonHotelContext _context;

        public StatisticRepository()
        {
            _context = new DbFourSeasonHotelContext();
        }

        public double GetTotalRevenue(int year)
        {

            double revenue = _context.Bookings.
                Where(b => b.CheckIn.Year == year && b.Status == "1").
                Sum(b => (double)b.TotalPrice);
            //return totalRevenue.total.ToString();
            return revenue;
        }

        public int GetActiveUser(int year)
        {
            var ActiveUser = _context.Bookings.
                Where(b => b.CheckIn.Year == year && b.Status == "1").
                Select(b => b.UserId).
                Distinct().
                Count();
            return ActiveUser;
        }

        public int GetTotalBooking(int year)
        {
            var TotalBooking = _context.Bookings.Count(b => b.CheckIn.Year == year);
            return TotalBooking;
        }

        public List<CollumChart> GetColumnChart(int year)
        {
            var monthlySales = from booking in _context.Bookings
                               where booking.CheckIn.Year == year && booking.Status == "1"
                               group booking by booking.CheckIn.Month into monthlyGroup
                               select new CollumChart
                               {
                                   Month = monthlyGroup.Key,
                                   Sales = monthlyGroup.Sum(booking => (double)booking.TotalPrice)
                               };
            var result = Enumerable.Range(1, 12)
                      .GroupJoin(monthlySales,
                                 month => month,
                                 sale => sale.Month,
                                 (month, sales) => new CollumChart
                                 {
                                     Month = month,
                                     Sales = sales.FirstOrDefault()?.Sales ?? 0
                                 })
                      .OrderBy(sale => sale.Month)
                      .ToList();

            return result;
        }

        public List<BarChart> GetBarChart(int year)
        {
            var salesByCategory = _context.Bookings
       .Where(b => b.CheckIn.Year == year && b.Status == "1")
       .Join(
           _context.BookingDetails,
           booking => booking.BookingId,
           bookingDetail => bookingDetail.BookingId,
           (booking, bookingDetail) => new { booking.TotalPrice, bookingDetail.RoomId }
       )
       .Join(
           _context.Rooms,
           bookingJoin => bookingJoin.RoomId,
           room => room.RoomId,
           (bookingJoin, room) => new { bookingJoin.TotalPrice, room.CategoryId }
       )
       .Join(
           _context.Categories,
           roomJoin => roomJoin.CategoryId,
           category => category.CategoryId,
           (roomJoin, category) => new { category.CategoryName, roomJoin.TotalPrice }
       )
       .GroupBy(r => r.CategoryName)
       .Select(g => new BarChart
       {
           Category = g.Key,
           Sales = (double)g.Sum(r => r.TotalPrice)
       })
       .ToList();

            return salesByCategory;
        }
    }
}
