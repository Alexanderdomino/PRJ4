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

        return obj.IsLogged ? LoggedDayTemplate : NotLoggedDayTemplate;

        //  if (obj is CalenderModel)
        //  {
        //      if (obj.IsLogged)
        //      {
        //          return LoggedDayTemplate;
        //      }
        //      
        //      return NotLoggedDayTemplate;
        //  }
        //
        //  if (obj is ReportModel)
        //  {
        //      if (obj.Unlocked)
        //      { 
        //          return UnlockedReportTemplate;
        //      }
        //
        //      return LockedReportTemplate;
        //  }
        //
        // return EmptyDayTemplate;
    }
}