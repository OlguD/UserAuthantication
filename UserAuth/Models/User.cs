namespace UserAuth.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; } = string.Empty;
    public required string Password { get; set; } = string.Empty;
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; }
    
    public override string ToString()
    {
        return $"User(Id={Id}, Username={Username}, Email={Email}, Name={Name}, Surname={Surname}, CreatedAt={CreatedAt}, UpdatedAt={UpdatedAt})";
    }
}