namespace OWP.TicketCenter.UnitTests;

public class CompareHelper
{
    public static bool Compare<T>(T Object1, T object2)
    {
        //Get the type of the object
        var type = typeof(T);

        //return false if any of the object is false
        if (Equals(Object1, default(T)) || Equals(object2, default(T)))
            return false;

        //Loop through each properties inside class and get values for the property from both the objects and compare
        foreach (var property in type.GetProperties())
        {
            if (property.Name != "ExtensionData")
            {
                var Object1Value = string.Empty;
                var Object2Value = string.Empty;
                if (type.GetProperty(property.Name).GetValue(Object1, null) != null)
                    Object1Value = type.GetProperty(property.Name).GetValue(Object1, null).ToString();
                if (type.GetProperty(property.Name).GetValue(object2, null) != null)
                    Object2Value = type.GetProperty(property.Name).GetValue(object2, null).ToString();
                if (Object1Value.Trim() != Object2Value.Trim())
                {
                    return false;
                }
            }
        }
        return true;
    }
}