using UserAuth.Models;

namespace UserAuth.Models;

public class ChangeRoleRequest
{
    public User UserChangingRole { get; set; }
    public string UserChanging { get; set; }
}
