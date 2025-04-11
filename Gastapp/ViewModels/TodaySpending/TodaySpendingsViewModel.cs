using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gastapp.Models;
using Gastapp.Services.Spending;
using Gastapp.Utils;
using Syncfusion.Maui.Calendar;

namespace Gastapp.ViewModels.TodaySpending
{
    public partial class TodaySpendingsViewModel : ObservableObject
    {
        private SpendingService _spendingService;

        private bool _isRangeSelected;
        [ObservableProperty] private bool _isExpanderExpanded;
        [ObservableProperty] private float _totalAmount = 0;
        [ObservableProperty] private float _calendarHeight = 150;
        [ObservableProperty] private int _numbersVisibleWeeks = 2;
        [ObservableProperty] private CalendarSelectionMode _selectionMode = CalendarSelectionMode.Single;
        [ObservableProperty] private MonthTemplate _template;
        [ObservableProperty] private DateTime _startDateRange = DateTime.UnixEpoch;
        [ObservableProperty] private DateTime _endDateRange = DateTime.UnixEpoch;
        [ObservableProperty] private CalendarDateRange _selectedDateRange = new(DateTime.Now, DateTime.Now);
        [ObservableProperty] private DateTime _maxDate = DateTime.Now;
        [ObservableProperty] private DateTime _minDate = DateTime.Now;
        [ObservableProperty] private DateTime _selectedDate = DateTime.Now;
        [ObservableProperty] ObservableCollection<Spending> _spendingList = new();
        [ObservableProperty] ObservableCollection<DateTime> _datesWithSpending = new();

        public TodaySpendingsViewModel(SpendingService spendingService)
        {
            _spendingService = spendingService;
            Template = new MonthTemplate();
        }

        partial void OnSpendingListChanged(ObservableCollection<Spending> value)
        {
            if (value.Count == 0)
            {
                TotalAmount = 0;
                return;
            }

            TotalAmount = value.Sum(x => (float)x.Amount);
        }

        [RelayCommand]
        private void ChangeExpanderExpanded()
        {
            IsExpanderExpanded = !IsExpanderExpanded;
        }

        [RelayCommand]
        private void ChangeToDayMode()
        {
            SelectionMode = CalendarSelectionMode.Single;
            SelectedDate = DateTime.Now;
            CalendarHeight = 150;
            NumbersVisibleWeeks = 2;
            GetSpendingByDate(SelectedDate);
        }

        [RelayCommand]
        private void ChangeToWeekMode()
        {
            SelectionMode = CalendarSelectionMode.Range;
            SelectedDate = DateTime.Now;
            StartDateRange = SelectedDate.AddDays(-(int)SelectedDate.DayOfWeek);
            EndDateRange = StartDateRange.AddDays(6);
            SelectedDateRange = new(StartDateRange, EndDateRange);
            CalendarHeight = 150;
            NumbersVisibleWeeks = 2;
            GetSpendingByDateRange(StartDateRange, EndDateRange);
        }

        [RelayCommand]
        private void ChangeToMonthMode()
        {
            SelectionMode = CalendarSelectionMode.Range;
            SelectedDate = DateTime.Now;
            StartDateRange = SelectedDate.AddDays(-(int)SelectedDate.DayOfWeek);
            EndDateRange = StartDateRange.AddMonths(1).AddDays(-1);
            SelectedDateRange = new(StartDateRange, EndDateRange);
            CalendarHeight = 250;
            NumbersVisibleWeeks = 5;
            GetSpendingByDateRange(StartDateRange, EndDateRange);
        }

        [RelayCommand]
        private void DeleteSpending(Spending item)
        {
            try
            {
                _spendingService.DeleteSpending(item);
                SpendingList.Remove(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        [RelayCommand]
        public void SelectionChanged(CalendarSelectionChangedEventArgs arg)
        {
            if (arg.NewValue is CalendarDateRange range)
            {
                _startDateRange = range.StartDate ?? DateTime.UnixEpoch;
                _endDateRange = range.EndDate ?? DateTime.UnixEpoch;

                if (_startDateRange != DateTime.UnixEpoch && _endDateRange != DateTime.UnixEpoch)
                    GetSpendingByDateRange(_startDateRange, _endDateRange);
            }

            if (arg.NewValue is DateTime date)
            {
                SelectedDate = date;
                GetSpendingByDate(SelectedDate);
            }
        }

        public void GetData()
        {
            DatesWithSpending = _spendingService.GetDatesWithSpending();

            GetSpendingByDate(SelectedDate);

            if (DatesWithSpending.Count > 0)
            {
                MaxDate = DatesWithSpending.Max();
                MinDate = DatesWithSpending.Min();
            }

            if (DateTime.Now > MaxDate)
                MaxDate = DateTime.Now;

            if (DateTime.Now < MinDate)
                MinDate = DateTime.Now;

            MinDate = MinDate.AddMonths(-1);
            MaxDate = MaxDate.AddMonths(1);
        }

        public void GetSpendingByDate(DateTime date)
        {
            SpendingList = _spendingService.GetTodaySpendings(date);
        }

        public void GetSpendingByDateRange(DateTime start, DateTime end)
        {
            SpendingList = _spendingService.GetSpendingByRange(start, end);
        }
    }
}