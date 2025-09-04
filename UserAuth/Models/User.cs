using System.ComponentModel.DataAnnotations;
namespace UserAuth.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public required string Username { get; set; } = string.Empty;
    
    [Required]
    public required string Password { get; set; } = string.Empty;
    
    [Required]
    public required string Email { get; set; }
    
    
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public string Role { get; set; } = "User";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; }
    
    public override string ToString()
    {
        return $"User(Id={Id}, Username={Username}, Email={Email}, Name={Name}, Surname={Surname}, Role={Role}, CreatedAt={CreatedAt}, UpdatedAt={UpdatedAt})";
    }
}