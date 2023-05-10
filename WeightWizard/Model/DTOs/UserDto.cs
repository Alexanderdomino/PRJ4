namespace WeightWizard.Model.DTOs;

public class UserDto
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Gender { get; set; }
    public decimal DesiredWeight { get; set; }
    public decimal InitialWeight { get; set; }
    public int Height { get; set; }
    public ICollection<DailyDataDto> DailyData { get; set; }
}