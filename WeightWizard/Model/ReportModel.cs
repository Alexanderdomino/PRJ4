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

    public List<CalenderModel> ReportDays { get; set; }

    public string WeeklyRapport { get; set; }
    
    public int RecommendedCalories { get; set; }
    
    public int RecommendedSteps { get; set; }
}