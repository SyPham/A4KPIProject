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

    public class DataReportDto
    {
        public DataReportDto()
        {
        }
        

        public H1H2ReportDto H1 { get; set; }
        public H1H2ReportDto H2 { get; set; }
        public double avg { get; set; }
    }
    public class KPICommentDto
    {
        public KPICommentDto()
        {
        }


        public string KPIComment { get; set; }
        public string Q1 { get; set; }
        public int Q1ID { get; set; }
        public string Q2 { get; set; }
        public int Q2ID { get; set; }
    }

    public class AttitudeCommentDto
    {
        public AttitudeCommentDto()
        {
        }


        public string AttitudeComment { get; set; }
        public string H1 { get; set; }
        public int H1ID { get; set; }
    }

    public class AtScoreDto
    {
        public AtScoreDto()
        {
        }


        public double L1 { get; set; }
        public int L1ID { get; set; }
        public double L2 { get; set; }
        public int L2ID { get; set; }
        public double FL { get; set; }
        public int FLID { get; set; }
    }

    public class ScoreKPIDto
    {
        public ScoreKPIDto()
        {
        }


        public double Self { get; set; }
        public double L1 { get; set; }
        public double L2 { get; set; }
        public double GHRSmartScore { get; set; }
    }
    public class CommentUpdateDto
    {
        public CommentUpdateDto()
        {
        }


        public int ID { get; set; }
        public string Content { get; set; }
    }

    public class AtScoreUpdateDto
    {
        public AtScoreUpdateDto()
        {
        }


        public int ID { get; set; }
        public int Point { get; set; }
    }

    public class ScoreSpecialDto
    {
        public ScoreSpecialDto()
        {
        }


        public double score { get; set; }
        public int ID { get; set; }
        public string comment { get; set; }
    }

    public class GHRDataDto
    {
        public GHRDataDto()
        {
        }

        public int Id { get; set; }
        public string OC { get; set; }
        public string FullName { get; set; }
        public double Score { get; set; }
    }

    public class DataGHRDto
    {
        public DataGHRDto()
        {
        }

        public object dataObjectH1 { get; set; }
        public object H1 { get; set; }
        public object kpicommentH1 { get; set; }
        public object attitudecomment { get; set; }
        public object attitudeScore { get; set; }
        public object kpiScore { get; set; }
        public object SpecialScore { get; set; }
        public object H2 { get; set; }
        public double H1Score { get; set; }
        public string dept { get; set; }
        public string Name { get; set; }
       
    }

    public class H1H2ReportDto
    {
        public H1H2ReportDto()
        {
        }

        public H1H2ReportDto(int halfyear, int quater, int year)
        {
            HalfYear = halfyear;
            Year = year;
            Quater = quater;
        }

        public string FullName { get; set; }
        public string OC { get; set; }
        public double L1Score { get; set; }
        public double L1 { get; set; }
        public string L1Comment { get; set; }
        public string FLHComment { get; set; }
        public string L1HComment { get; set; }
        public string L1H2Comment { get; set; }
        public string L1Q1Comment { get; set; }
        public string L1Q2Comment { get; set; }
        public double L2Score { get; set; }
        public double L2 { get; set; }
        public double A_total { get; set; }
        public double B_total { get; set; }
        public double selfScore { get; set; }
        public double B_selfScore { get; set; }
        public double B_L1 { get; set; }
        public double B_L2 { get; set; }
        public double B_Smart { get; set; }
        public double Smart { get; set; }
        public double C_total { get; set; }
        public double D_total { get; set; }
        public double total { get; set; }
        public string L2Comment { get; set; }
        public string L0SelfScoreComment { get; set; }
        public string L2HComment { get; set; }
        public string L2H2Comment { get; set; }
        public string L2Q1Comment { get; set; }
        public string L2Q2Comment { get; set; }
        public double FLScore { get; set; }
        public double SmartScore { get; set; }
        public double SpecialScore { get; set; }
        public string SpecialComment { get; set; }
        public int HalfYear { get; set; }
        public int Quater { get; set; }
        public int Year { get; set; }
    }
}
