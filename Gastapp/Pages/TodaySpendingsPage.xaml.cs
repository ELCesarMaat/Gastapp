using Gastapp.ViewModels;
using Gastapp.ViewModels.TodaySpending;
using Syncfusion.Maui.Calendar;

namespace Gastapp.Pages;

public partial class TodaySpendingsPage : ContentPage
{
    private TodaySpendingsViewModel _vm;

    public TodaySpendingsPage(TodaySpendingsViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
        calendar.MonthView.SpecialDayPredicate += SpecialDayPredicate;
    }

    private CalendarIconDetails SpecialDayPredicate(DateTime arg)
    {
        if (_vm.DatesWithSpending.Any(x => DateOnly.FromDateTime(x) == DateOnly.FromDateTime(arg)))
        {
            CalendarIconDetails iconDetails = new CalendarIconDetails();
            iconDetails.Icon = CalendarIcon.Dot;
            iconDetails.Fill = Colors.Red;
            return iconDetails;
        }

        return null;
    }

    protected override void OnAppearing()
    {
        _vm.GetData();
        base.OnAppearing();
    }
}