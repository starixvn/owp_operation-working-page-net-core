namespace OWP.AutoAssignment.Models.InternalApi;

using Epplus.Mapper.Annotations;
using OfficeOpenXml.Attributes;

[EpplusTable]
public record ExportExcelModel
{
    [Cell("B1")]
    public string Date { get; init; }

    [EpplusIgnore]
    public IList<ExportMatchModel> exportMatchModels
    {
        get;
        set;
    } = new List<ExportMatchModel>();
}