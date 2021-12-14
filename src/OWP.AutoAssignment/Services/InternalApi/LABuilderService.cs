using OWP.AutoAssignment.Criteria.InternalApi;
using OWP.AutoAssignment.Models.InternalApi;
using OWP.AutoAssignment.Utils.XMLExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OWP.AutoAssignment.Services.InternalApi
{
    public interface LABuilderService
    {
        Task<object> Build(IList<LAExportQueried> info);
    }

    public class LABuilderServiceImp : LABuilderService
    {
        private readonly Repository repository;

        public LABuilderServiceImp(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<object> Build(IList<LAExportQueried> info)
        {
            var criteria = new GetLAExportInfoCriteria
            {
                Xml = info.ToXml().ToString(SaveOptions.DisableFormatting)
            };

            return await repository.GetLAExportInfo(criteria);
        }
    }
}