namespace OWP.PreMatchNotification.LinkedMatch.Services
{
    using OWP.PreMatchNotification.LinkedMatch.Models;
    using OWP.PreMatchNotification.Models;
    using OWP.PreMatchNotification.Services;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ChangedMatchStatusGenerator
    {
        Task<List<ChangedValue>> Create(ChangedSnapshot changedSnapshot);
    }

    public class ChangedMatchStatusGeneratorImp : ChangedMatchStatusGenerator
    {       

        public async Task<List<ChangedValue>> Create(ChangedSnapshot changedSnapshot)
        {
            
            return new List<ChangedValue>();
        }
    }
}
