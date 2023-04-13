namespace OWP.AssigneeSuggestionList.Models;

public class WorkingSchedule
{
    public UserInfo UserInfo { get; set; }
    public AssignmentCell Yesterday_02h { get; set; }
    public AssignmentCell Yesterday_03h { get; set; }
    public AssignmentCell Yesterday_04h { get; set; }
    public AssignmentCell Yesterday_05h { get; set; }
    public AssignmentCell Yesterday_06h { get; set; }
    public AssignmentCell Yesterday_07h { get; set; }
    public AssignmentCell Yesterday_08h { get; set; }
    public AssignmentCell Yesterday_09h { get; set; }
    public AssignmentCell Yesterday_10h { get; set; }
    public AssignmentCell Yesterday_11h { get; set; }
    public AssignmentCell Today_12h { get; set; }
    public AssignmentCell Today_13h { get; set; }
    public AssignmentCell Today_14h { get; set; }
    public AssignmentCell Today_15h { get; set; }
    public AssignmentCell Today_16h { get; set; }
    public AssignmentCell Today_17h { get; set; }
    public AssignmentCell Today_18h { get; set; }
    public AssignmentCell Today_19h { get; set; }
    public AssignmentCell Today_20h { get; set; }
    public AssignmentCell Today_21h { get; set; }
    public AssignmentCell Today_22h { get; set; }
    public AssignmentCell Today_23h { get; set; }
    public AssignmentCell Today_00h { get; set; }
    public AssignmentCell Today_01h { get; set; }
    public AssignmentCell Today_02h { get; set; }
    public AssignmentCell Today_03h { get; set; }
    public AssignmentCell Today_04h { get; set; }
    public AssignmentCell Today_05h { get; set; }
    public AssignmentCell Today_06h { get; set; }
    public AssignmentCell Today_07h { get; set; }
    public AssignmentCell Today_08h { get; set; }
    public AssignmentCell Today_09h { get; set; }
    public AssignmentCell Today_10h { get; set; }
    public AssignmentCell Today_11h { get; set; }
    public AssignmentCell Tomorrow_12h { get; set; }
    public AssignmentCell Tomorrow_13h { get; set; }
    public AssignmentCell Tomorrow_14h { get; set; }
    public AssignmentCell Tomorrow_15h { get; set; }
    public AssignmentCell Tomorrow_16h { get; set; }
    public List<AssignmentModel> AssignmentModels { get; set; }
}

public class AssignmentCell
{
    public string Text { get; set; }

    public string TextColor { get; set; }

    public string Color { get; set; }
}