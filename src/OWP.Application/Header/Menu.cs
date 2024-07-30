
namespace OWP.Application.Header;
public class Menu
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Path { get; set; }

    public string IconCssClass { get; set; }

    public List<Menu> Children { get; set; }
}