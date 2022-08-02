namespace OWP.Models
{
    public class PermissionModel
    {
        public PermissionModel()
            : this(false, false)
        {
        }

        public PermissionModel(bool isRead, bool isHost)
        {
            IsRead = isRead;
            IsHost = isHost;
        }

        public bool IsRead { get; }

        public bool IsHost { get; }
    }
}
