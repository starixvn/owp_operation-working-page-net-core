using System.Reflection;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Helper
{
    public static class CreateWorkingScheduleHelper
    {
        public static WorkingSchedule CreateWorkingSchedule(WorkingScheduleCreatorModel creatorModel)
        {
            var schedule = new WorkingSchedule
            {
                UserInfo = new()
                {
                    UserId = creatorModel.UserId
                },
            };

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