using WeightWizard.Model.Interfaces;

namespace WeightWizard.Model;

public class ReportModel : ICalenderItems
{
    public ReportModel()
    {
        IsLogged = false;
        Unlocked = false;
    }
    
    public bool IsLogged { get; set; }
    public bool Unlocked { get; set; }

    public string ClickAble { get; set; }
}