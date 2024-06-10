namespace OWP.Application.Authorize;
public class AuthorizeResult
{
    public int UserId { get; set; }

    public string? Username { get; set; }

    public bool HasRead { get; set; }

    public bool HasWrite { get; set; }

    public int SiteId { get; set; }
}
