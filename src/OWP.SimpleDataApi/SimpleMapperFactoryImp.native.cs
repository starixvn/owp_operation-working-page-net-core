using Olympus.SimpleDataApi;
using OWP.SimpleDataApi.Mapper;

namespace OWP.SimpleDataApi
{
    public class SimpleMapperFactoryImp : SimpleMapperFactoryBase
    {
        static SimpleMapperFactoryImp()
        {
            AddSimpleMapperService("soccer-early-get-match", typeof(GoalIconMapperService));
        }
    }
}
