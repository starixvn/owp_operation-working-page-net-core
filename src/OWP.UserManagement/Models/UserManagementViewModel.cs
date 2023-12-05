namespace OWP.UserManagement.Models;

using System.Collections.Generic;
using OWP.Models;

public class UserManagementViewModel
{
    public UserManagementViewModel()
    {
        Users = new List<User>();
    }

    public IEnumerable<User> Users { get; set; }

    public IEnumerable<UserColumnModel> UserColumnModels { get; set; }
}