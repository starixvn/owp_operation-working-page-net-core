namespace OWP.SimpleDataApi;

using Mapper;
using Olympus.SimpleDataApi;

public class SimpleMapperFactoryImp : SimpleMapperFactoryBase
{
    static SimpleMapperFactoryImp()
    {
        AddSimpleMapperService("soccer-early-get-match", typeof(GoalIconMapperService));
    }
}