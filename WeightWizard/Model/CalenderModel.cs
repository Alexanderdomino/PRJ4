using WeightWizard.Model.Interfaces;

namespace WeightWizard.Model;

public class CalenderModel : ICalenderItems
{
    public CalenderModel()
    {
        IsLogged = false;
        Unlocked = false;
    }
    public DateTime Date { get; set; }
    public bool IsLogged { get; set; }
    public bool Unlocked { get; set; }
    
    public decimal MorningWeight { get; set; }
    
    public int CalorieIntake { get; set; }
    
    public int Steps { get; set; }
}