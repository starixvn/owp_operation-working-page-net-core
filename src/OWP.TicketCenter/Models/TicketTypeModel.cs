namespace OWP.TicketCenter.Models
{
    public class TicketTypeModel
    {
        public List<ParentType> Parent { get; set; }

        public List<SubType> Child { get; set; }
    }

    public class ParentType
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class SubType
    {
        public int ParentId { get; set; }

        public int SubTypeId { get; set; }

        public string SubTypeName { get; set; }
    }
}