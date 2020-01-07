using Abp.Authorization;
using Lpb.WebPortal.Authorization.Roles;
using Lpb.WebPortal.Authorization.Users;

namespace Lpb.WebPortal.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
