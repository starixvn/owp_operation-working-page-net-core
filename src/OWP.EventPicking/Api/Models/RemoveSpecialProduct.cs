namespace OWP.EventPicking.Api.Models;
public class RemoveSpecialProduct
{
    public string ParentId { get; set; }

    public string MinorBettypeId { get; set; }

    /// <summary>
    /// JSON string mapping ParentId to existing MinorBettypeIds
    /// Format: {"parentId1": "1,2", "parentId2": "3,4"}
    /// </summary>
    public string ExistingMinorBettypeIds { get; set; }
}
