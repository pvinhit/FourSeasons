namespace be.Models
{
    public class Statistic
    {
        public double TotalRevenue { get; set; } 
        public int ActiveUser { get; set;}
        public int TotalBooking { get; set; }
        public IList<CollumChart> CollumChart { get; set;} = new List<CollumChart>();
       public IList<BarChart> BarChart { get; set; } = new List<BarChart>();
    }
}
