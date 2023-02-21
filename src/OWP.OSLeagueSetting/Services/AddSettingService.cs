using AutoMapper;
using OWP.OSLeagueSetting.Criteria;
using OWP.OSLeagueSetting.Models;

namespace OWP.OSLeagueSetting.Services;

public interface AddSettingService
{
    Task Execute(SettingDetail model);
}

public class AddSettingServiceImp : AddSettingService
{
    private readonly Repository repository;

    private readonly IMapper mapper;

    public AddSettingServiceImp(Repository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task Execute(SettingDetail model)
    {
        var criteria = mapper.Map<AddSettingCriteria>(model);
        await repository.AddOrEditSetting(criteria);
    }
}