using System.Reflection;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Helper
{
    public static class UpdateWorkingScheduleHelper
    {
        public static WorkingSchedule UpdateWorkingSchedule(WorkingSchedule schedule, WorkingScheduleUpdateModel creatorModel)
        {
            var objType = schedule.GetType();
            foreach (var name in creatorModel.PropertiesName)
            {
                var propertyInfo = objType.GetProperty(name, BindingFlags.Public);
                if (propertyInfo is not null)
                {
                    propertyInfo.SetValue(schedule, creatorModel.AssignmentCell);
                }
            }
            return schedule;
        }
    }
}