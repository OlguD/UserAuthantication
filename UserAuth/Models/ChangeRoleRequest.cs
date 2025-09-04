using UserAuth.Models;

namespace UserAuth.Models;

public class ChangeRoleRequest
{
    public string UserChangingRole { get; set; }
    public string UserChanging { get; set; }
    public string Role { get; set; }
}
