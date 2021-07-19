using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class Q1Q3ReportDto
    {
        public Q1Q3ReportDto()
        {
        }

        public Q1Q3ReportDto(int quarter, int year)
        {
            Quarter = quarter;
            Year = year;
        }

        public string FullName { get; set; }
        public string OC  { get; set; }
        public double L1Score { get; set; }
        public string L1Comment { get; set; }
        public double L2Score { get; set; }
        public string L2Comment { get; set; }
        public double SmartScore { get; set; }
        public int Quarter { get; set; }
        public int Year { get; set; }
    }

    public class H1H2ReportDto
    {
        public H1H2ReportDto()
        {
        }

        public H1H2ReportDto(int halfyear, int year)
        {
            HalfYear = halfyear;
            Year = year;
        }

        public string FullName { get; set; }
        public string OC { get; set; }
        public double L1Score { get; set; }
        public string L1Comment { get; set; }
        public double L2Score { get; set; }
        public string L2Comment { get; set; }
        public double FLScore { get; set; }
        public double SmartScore { get; set; }
        public int HalfYear { get; set; }
        public int Year { get; set; }
    }
}
