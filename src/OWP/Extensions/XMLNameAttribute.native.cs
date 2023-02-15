namespace OWP.Extensions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public sealed class XMLNameAttribute : Attribute
{
    public XMLNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}