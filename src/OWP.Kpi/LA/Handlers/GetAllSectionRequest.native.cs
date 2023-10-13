
using MediatR;
using OWP.Kpi.LA.Models;
using OWP.Kpi.LA.Services;

namespace OWP.Kpi.LA.Handlers;
public class GetAllSectionRequest:IRequest<IEnumerable<SectionInfo>>
{
    public int UserId { get; set; }
}
public class GetAllSectionHandler : IRequestHandler<GetAllSectionRequest, IEnumerable<SectionInfo>>
{
    private readonly Repository repository;

    public GetAllSectionHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<SectionInfo>> Handle(GetAllSectionRequest request,
        CancellationToken cancellationToken)
    {
        var sections = (await repository.GetAllSection(request.UserId)).ToList();
        AddEmptySection(sections);
        return sections;
    }

    private void AddEmptySection(List<SectionInfo> sections)
    {
        var isExitSection = sections.Any(x => x.IsContainUser);
        sections.Add(new SectionInfo
        {
            SectionId = 0,
            SectionName = "None",
            IsContainUser = !isExitSection 
        });
    }
}
