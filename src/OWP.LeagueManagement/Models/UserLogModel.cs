namespace OWP.LeagueManagement.Models;

using System;

public class UserLogModel
{
    public string Action { get; set; }

    public string DataOld { get; set; }

    public string Data { get; set; }

    public DateTime ModifiedDate { get; set; }

    public string ModifiedBy { get; set; }
}