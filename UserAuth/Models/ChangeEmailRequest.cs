using UserAuth.Models;

namespace UserAuth.Models;

public class ChangeEmailRequest
{
    public string Username { get; set; }
    public string NewEmail { get; set; }
}
