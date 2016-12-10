using System;
using OpenB.Core.ACL;

namespace OpenB.Modeling
{
    public class ModelAuthorizationService : IModelAuthorizationService
    {
        public bool IsUserAuthorizedForModel(User user, Permissions permissions, IAuthorizebleModel model)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (model == null) throw new ArgumentNullException("model");

            if (user.UserGroups.Contains(model.UserGroup))
            {
                // If 
                if ((model.GroupPermissions & permissions) == permissions)
                {
                    return true;
                }

                // If 
                if ((model.UserPermissions & permissions) == permissions)
                {
                    return true;
                }
            }
            return false;
        }
    }
}