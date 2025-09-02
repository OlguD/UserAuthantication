using UserAuth.Models;

namespace UserAuth.Models;

public class ChangePasswordRequest
{
    public User User { get; set; }
    public string NewPassword { get; set; }
}
