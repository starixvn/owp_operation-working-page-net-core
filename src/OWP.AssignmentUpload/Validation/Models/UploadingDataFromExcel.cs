namespace OWP.AssignmentUpload.Validation.Models;

using System;
using System.Collections.Generic;

public class UploadingDataFromExcel
{
    public DateTime EventDate { get; set; }

    public List<AssignmentModelInExcel> Assignments { get; set; }

    public List<string> Traders { get; set; }
}