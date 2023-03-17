using WeightWizard_test.Model.Interfaces;

namespace WeightWizard_test.Model;

public class ReportModel : ICalenderItems
{
    public ReportModel()
    {
        IsLogged = false;
        Unlocked = false;
    }
    public bool IsLogged { get; set; }
    public bool Unlocked { get; set; }
}