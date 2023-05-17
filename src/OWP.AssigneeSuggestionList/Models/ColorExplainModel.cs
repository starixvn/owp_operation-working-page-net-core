namespace OWP.AssigneeSuggestionList.Models;

public class ColorExplainModel
{
    public string Label { get; set; }

    public ColorExplainModel(string label, string backgroundColor, string textColor)
    {
        Label = label;
        BackgroundColor = backgroundColor;
        TextColor = textColor;
    }

    public string BackgroundColor { get; set; }
    public string TextColor { get; set; }
}