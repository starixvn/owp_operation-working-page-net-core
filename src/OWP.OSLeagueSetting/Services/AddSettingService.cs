using OWP.OSLeagueSetting.Models;
using AutoMapper;
using OWP.OSLeagueSetting.Criteria;

namespace OWP.OSLeagueSetting.Services;

public interface AddSettingService
{
    Task Execute(AddSetting model);
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

    public async Task Execute(AddSetting model)
    {
        var criteria = mapper.Map<AddSettingCriteria>(model);
        await repository.AddOrEditSetting(criteria);
    }
}