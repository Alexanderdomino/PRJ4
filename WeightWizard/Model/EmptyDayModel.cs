using WeightWizard.Model.Interfaces;

namespace WeightWizard.Model;

public class EmptyDayModel : ICalenderItems
{
    public EmptyDayModel()
    {
        IsLogged = false;
        Unlocked = false;
    }
    
    public bool IsLogged { get; set; }
    public bool Unlocked { get; set; }
}