namespace OWP.Kpi.LA.Models;

public class QueryModel
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public string SectionIds { get; set; }

    public bool ActiveUser { get; set; }

    public bool ResignUser { get; set; }

    public override string ToString()
    => $"FromDate: {FromDate}, ToDate: {ToDate}, SectionIds: {SectionIds}, ActiveUser: {ActiveUser}, ResignUser: {ResignUser}";
}