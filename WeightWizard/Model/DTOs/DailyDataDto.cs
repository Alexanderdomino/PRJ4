namespace WeightWizard.Model.DTOs;

public class DailyDataDto
{
    public int DailyDataId { get; set; }
    public int UserId { get; set; }
    public decimal MorningWeight { get; set; }
    public int CalorieIntake { get; set; }
    public decimal DesiredWeight { get; set; }
    public DateTime Date { get; set; }
    public int Steps { get; set; }
}