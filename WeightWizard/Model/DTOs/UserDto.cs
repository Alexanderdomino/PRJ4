namespace WeightWizard.Model.DTOs;

public class UserDto
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public decimal DesiredWeight { get; set; }

    public decimal InitialWeight { get; set; }

    public int Height { get; set; }
    
}