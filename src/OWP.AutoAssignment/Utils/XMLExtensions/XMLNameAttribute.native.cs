namespace OWP.AutoAssignment.Utils.XMLExtensions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed class XMLNameAttribute : Attribute
    {
        private readonly string name;

        public XMLNameAttribute(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }
    }
}