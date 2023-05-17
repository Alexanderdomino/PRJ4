namespace WeightWizard.Selectors;

using Model;
using Model.Interfaces;

public class CalenderDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate EmptyDayTemplate { get; set; }
    
    public DataTemplate LoggedDayTemplate { get; set; }
    public DataTemplate NotLoggedDayTemplate { get; set; }
    
    public DataTemplate LockedReportTemplate { get; set; }
    public DataTemplate UnlockedReportTemplate { get; set; }
    
    public DataTemplate DayNameTemplate { get; set; }
    
    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        var obj = (ICalenderItems)item;

        switch (obj)
        {
            case CalenderModel:
                return obj.IsLogged ? LoggedDayTemplate : NotLoggedDayTemplate;
            case ReportModel:
                return obj.Unlocked ? UnlockedReportTemplate : LockedReportTemplate;
            case EmptyDayModel:
                return EmptyDayTemplate;
            case DayNameModel:
                return DayNameTemplate;
            default:
                return EmptyDayTemplate;
        }
    }
}