using OWP.AutoAssignment.Criteria.InternalApi;
using OWP.AutoAssignment.Models.InternalApi;
using OWP.AutoAssignment.Utils.XMLExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OWP.AutoAssignment.Services.InternalApi
{
    public interface TraderBuilderService
    {
        Task<object> Build(IList<TraderExportQueried> info);
    }

    public class TraderBuilderServiceImp : TraderBuilderService
    {
        private readonly Repository repository;

        public TraderBuilderServiceImp(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<object> Build(IList<TraderExportQueried> info)
        {
            var criteria = new GetTraderExportInfoCriteria
            {
                Xml = info.ToXml().ToString(SaveOptions.DisableFormatting)
            };

            return await repository.GetTraderExportInfo(criteria);
        }
    }
}