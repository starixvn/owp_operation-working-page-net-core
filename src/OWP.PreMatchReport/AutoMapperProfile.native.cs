using AutoMapper;
using OWP.PreMatchReport.Models;
namespace OWP.PreMatchReport
{
    public class AutoMapperProfile
    {
        public AutoMapperProfile(){
            CreateMap<ReportModel, ReportLeagueModel>();
        }
    }
}