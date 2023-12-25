namespace OWP.Report.Models;

public class QueryModel
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public bool ActiveUser { get; set; }

    public bool ResignUser { get; set; }

    public override string ToString()
   => $"FromDate: {FromDate}, ToDate: {ToDate}, ActiveUser: {ActiveUser}, ResignUser: {ResignUser}";
}