namespace OWP.SimpleDataApi.Services;

public interface SimpleMapperService
{
    void Decorate(dynamic obj);
}

public class GoalIconMapperService : SimpleMapperService
{
    public void Decorate(dynamic obj)
    {
        // Method intentionally left empty.
    }
}