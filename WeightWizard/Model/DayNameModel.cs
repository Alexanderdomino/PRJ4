using WeightWizard.Model.Interfaces;

namespace WeightWizard.Model;

public class DayNameModel : ICalenderItems
{
    public DayNameModel()
    {
        IsLogged = false;
        Unlocked = false;
    }
    public DateTime Date { get; set; }
    public bool IsLogged { get; set; }
    public bool Unlocked { get; set; }
}