using UserAuth.Models;

namespace UserAuth.Models;

public class ChangeEmailRequest
{
    public User User { get; set; }
    public string NewEmail { get; set; }
}
