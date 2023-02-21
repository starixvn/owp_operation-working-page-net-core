using AutoMapper;
using OWP.LiveDetail.Assignment;
using OWP.LiveDetail.MatchInfo;
using QueryCriteria = OWP.LiveDetail.Assignment.QueryCriteria;

namespace OWP.LiveDetail;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<QueryModel, QueryCriteria>();
        CreateMap<CreateModel, CreateCriteria>();
        CreateMap<UpdateModel, UpdateCriteria>();
        CreateMap<DeleteModel, DeleteCriteria>();
        CreateMap<ValidateModel, ValidateCriteria>();

        CreateMap<MatchSetting.QueryModel, MatchSetting.QueryCriteria>();

        CreateMap<MatchNote.QueryModel, MatchNote.QueryCriteria>();
        CreateMap<MatchNote.CreateModel, MatchNote.CreateCriteria>();
        CreateMap<MatchNote.UpdateModel, MatchNote.UpdateCriteria>();
        CreateMap<MatchNote.DeleteModel, MatchNote.DeleteCriteria>();

        CreateMap<MatchOdd.QueryModel, MatchOdd.QueryCriteria>();
        CreateMap<MatchOdd.CreateModel, MatchOdd.CreateCriteria>();
        CreateMap<MatchOdd.UpdateModel, MatchOdd.UpdateCriteria>();
        CreateMap<MatchOdd.DeleteModel, MatchOdd.DeleteCriteria>();

        CreateMap<UpdateMatchInfoModel, UpdateMatchInfoCriteria>();
        CreateMap<UpdateProviderInfoModel, UpdateProviderInfoCriteria>();
        CreateMap<UpdateTradingInfoModel, UpdateTradingInfoCriteria>();
        CreateMap<UpdateMatchStatusModel, UpdateMatchStatusCriteria>();
        CreateMap<ValidatePhoneModel, ValidatePhoneCriteria>();

        CreateMap<MatchSetting.CreateModel, MatchSetting.CreateCriteria>();
        CreateMap<MatchSetting.UpdateModel, MatchSetting.UpdateCriteria>();
        CreateMap<MatchSetting.DeleteModel, MatchSetting.DeleteCriteria>();
    }
}