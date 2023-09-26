using be.Models;
using be.Services;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;

namespace be.Controllers
{
    using System.Data;

    
    public class StatisticController : Controller
    {
        private readonly IStatisticService _statisticService;

        public StatisticController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("api/statistic/{year}")]
        public ActionResult getStatistic(int year)
        {
            Statistic statistic = new Statistic();
            statistic.TotalBooking = _statisticService.GetTotalBooking(year);
            statistic.ActiveUser = _statisticService.GetActiveUser(year);
            statistic.TotalRevenue = _statisticService.GetTotalRevenue(year);
            statistic.CollumChart = _statisticService.GetColumnChart(year); 
            statistic.BarChart = _statisticService.GetBarChart(year);

            if (statistic == null)
            {
                return NotFound();
            }
            return Ok(statistic); 
        }
        [HttpGet("api/exportExcel/{year}")]
        public ActionResult ExportExcel(int year)
        {
            var result1 = GetDataColumnChart(year);
            var result2 = GetTypeRoomRevenue(year);
            var result3 = GetTotal(year);

            using (XLWorkbook wb = new XLWorkbook())
            {
                var sheet1 = wb.AddWorksheet(result1, "Monthly Total Revenue");
                wb.AddWorksheet(result2);
                wb.AddWorksheet(result3);

                sheet1.Column(1).Style.Font.FontColor = XLColor.Red;
              
                sheet1.Row(1).CellsUsed().Style.Fill.BackgroundColor = XLColor.Black;
                sheet1.Row(1).Style.Font.FontColor = XLColor.White;
                sheet1.Row(1).Style.Font.Bold = true;
                sheet1.Row(1).Style.Font.Shadow = true;
                sheet1.Row(1).Style.Font.Underline = XLFontUnderlineValues.Single;
                sheet1.Row(1).Style.Font.VerticalAlignment = XLFontVerticalTextAlignmentValues.Superscript;
                sheet1.Row(1).Style.Font.Italic = true;
                sheet1.Rows(2, 3).Style.Font.FontColor = XLColor.AshGrey;
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Revenue Statistic of {year}.xlsx");
                }
            }
        }
        [NonAction]
        private DataTable GetDataColumnChart(int year)
        {
            Statistic statistic = new Statistic();
            statistic.CollumChart = _statisticService.GetColumnChart(year);
            IList<CollumChart> listColums = statistic.CollumChart; 

            DataTable dt = new DataTable();
            dt.TableName = "Monthly Total Revenue";
            dt.Columns.Add("Jan", typeof(string));
            dt.Columns.Add("Feb", typeof(string));
            dt.Columns.Add("March", typeof(string));
            dt.Columns.Add("Apr", typeof(string));
            dt.Columns.Add("May", typeof(string));
            dt.Columns.Add("Jun", typeof(string));
            dt.Columns.Add("Jul", typeof(string));
            dt.Columns.Add("Aug", typeof(string));
            dt.Columns.Add("Sept", typeof(string));
            dt.Columns.Add("Oct", typeof(string));
            dt.Columns.Add("Nov", typeof(string));
            dt.Columns.Add("Dec", typeof(string));

            
            dt.Rows.Add(GetSalesByMonth(statistic.CollumChart,1), GetSalesByMonth(statistic.CollumChart, 2),
                GetSalesByMonth(statistic.CollumChart, 3), GetSalesByMonth(statistic.CollumChart, 4), GetSalesByMonth(statistic.CollumChart, 5),
                GetSalesByMonth(statistic.CollumChart, 6), GetSalesByMonth(statistic.CollumChart,7), GetSalesByMonth(statistic.CollumChart, 8), 
                GetSalesByMonth(statistic.CollumChart, 9),GetSalesByMonth(statistic.CollumChart, 10), GetSalesByMonth(statistic.CollumChart, 11), 
                GetSalesByMonth(statistic.CollumChart, 12)); 
            return dt;
        }
        [NonAction]
        private DataTable GetTypeRoomRevenue(int year)
        {
            Statistic statistic = new Statistic();
            statistic.BarChart = _statisticService.GetBarChart(year);
            DataTable dt = new DataTable();
            dt.TableName = "RoomType Total Revenue";
            dt.Columns.Add("Deluxe", typeof(string));
            dt.Columns.Add("Double", typeof(string));
            dt.Columns.Add("Suit", typeof(string));
            dt.Columns.Add("Superior Twin", typeof(string));

            dt.Rows.Add(GetSalesByCategory(statistic.BarChart, "Deluxe"),
                GetSalesByCategory(statistic.BarChart, "Double"),
                GetSalesByCategory(statistic.BarChart, "Suit"),
                GetSalesByCategory(statistic.BarChart, "Superior Twin"));
            return dt;
        }
        [NonAction]
        private DataTable GetTotal(int year)
        {
            Statistic statistic = new Statistic();
            statistic.TotalBooking = _statisticService.GetTotalBooking(year);
            statistic.ActiveUser = _statisticService.GetActiveUser(year);
            statistic.TotalRevenue = _statisticService.GetTotalRevenue(year);

            DataTable dt = new DataTable();
            dt.TableName = "Total";
            dt.Columns.Add("Total booking", typeof(string));
            dt.Columns.Add("Active user", typeof(string));
            dt.Columns.Add("Total revenue", typeof(string));

            dt.Rows.Add(statistic.TotalBooking, statistic.ActiveUser, statistic.TotalRevenue);

            return dt;
        }

        public double GetSalesByMonth(IList<CollumChart> charts, int month)
        {
            double sale = 0;
            for (int i = 0; i < charts.Count; i++)
            {
                if (charts[i].Month == month)
                {
                    sale = charts[i].Sales;
                }
                
            }
            return sale; 
        }


        public double GetSalesByCategory(IList<BarChart> charts, string category)
        {
            double sale = 0;
            for (int i = 0; i < charts.Count; i++)
            {
                if (charts[i].Category == category)
                {
                    sale = charts[i].Sales;
                }
            }
            return sale;
        }


    }
}
