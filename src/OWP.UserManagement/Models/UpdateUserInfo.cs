using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.UserManagement.Models;

public class UpdateUserInfo
{
    public int ActorId { get; set; }

    public int UserId { get; set; }

    public string ColumnName { get; set; }

    public string Value { get; set; }
}