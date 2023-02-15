namespace OWP.OSLeagueSetting.Services;

using AutoMapper;
using Criteria;
using Models;

public interface EditSettingService
{
    Task Execute(SettingDetail model);
}

public class EditSettingServiceImp : EditSettingService
{
    private readonly Repository repository;

    private readonly IMapper mapper;

    public EditSettingServiceImp(Repository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task Execute(SettingDetail model)
    {
        var criteria = mapper.Map<EditSettingCriteria>(model);
        await repository.AddOrEditSetting(criteria);
    }
}