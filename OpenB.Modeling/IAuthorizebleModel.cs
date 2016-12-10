using System.Text.RegularExpressions;
using OpenB.Core;
using OpenB.Core.ACL;

namespace OpenB.Modeling
{
    public interface IAuthorizebleModel
    {
        User User { get; }
        UserGroup UserGroup { get; }
        Permissions UserPermissions { get; }
        Permissions GroupPermissions { get; }
    }
}