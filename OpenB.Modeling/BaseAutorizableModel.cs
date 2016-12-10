using OpenB.Core.ACL;

namespace OpenB.Modeling
{
    public abstract class BaseAutorizableModel : BaseModel, IAuthorizebleModel 
    {
        public User User { get; private set; }
        public UserGroup UserGroup { get; private set; }
        public Permissions UserPermissions { get; set; }
        public Permissions GroupPermissions { get; set; }

        protected BaseAutorizableModel(string key, string name, string description, User user, Permissions userPermissions, Permissions groupPermissions) : base(key, name, description)
        {
            User = user;
            UserPermissions = userPermissions;
            GroupPermissions = groupPermissions;
        }
    }
}