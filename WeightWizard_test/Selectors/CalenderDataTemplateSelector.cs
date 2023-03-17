namespace WeightWizard_test.Selectors;

using Model;
using Model.Interfaces;

public class CalenderDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate EmptyDayTemplate { get; set; }
    
    public DataTemplate LoggedDayTemplate { get; set; }
    public DataTemplate NotLoggedDayTemplate { get; set; }
    
    public DataTemplate LockedReportTemplate { get; set; }
    public DataTemplate UnlockedReportTemplate { get; set; }
    
    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        var obj = (ICalenderItems)item;

        switch (obj)
        {
            case CalenderModel:
                return obj.IsLogged ? LoggedDayTemplate : NotLoggedDayTemplate;
            // case ReportModel:
            //     return obj.Unlocked ? UnlockedReportTemplate : LockedReportTemplate;
            case EmptyDayModel:
                return EmptyDayTemplate;
            
            default:
                return EmptyDayTemplate;
        }
    }
}