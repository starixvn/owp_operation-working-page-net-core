namespace OWP.PreMatchNotification.LinkedMatch.Services
{
    using OWP.PreMatchNotification.LinkedMatch.Models;
    using OWP.PreMatchNotification.Models;
    using OWP.PreMatchNotification.Services;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ChangedMatchStatusGenerator
    {
        List<ChangedValue> Create(ChangedSnapshot changedSnapshot, Employee employees);
    }

    public class ChangedMatchStatusGeneratorImp : ChangedMatchStatusGenerator
    {

        public List<ChangedValue> Create(ChangedSnapshot changedSnapshot, Employee employees)
        {
            
            return new List<ChangedValue>();
        }
    }
}
