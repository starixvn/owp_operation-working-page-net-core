namespace OWP.UserManagement.Models
{
    using System.Collections.Generic;
    using OWP.Models;

    public class UserManagementViewModel : Pagination
    {
        public UserManagementViewModel()
        {
            Users = new List<User>();
        }

        public IEnumerable<User> Users { get; set; }

        public IEnumerable<ReferenceResource> LevelOfLiveTrading { get; set; }

        public IEnumerable<ReferenceResource> LevelOfLiveAssistant { get; set; }

        public IEnumerable<ReferenceResource> LevelOfOtherSection { get; set; }

        public IEnumerable<UserColumnModel> UserColumnModels { get; set; }
    }
}