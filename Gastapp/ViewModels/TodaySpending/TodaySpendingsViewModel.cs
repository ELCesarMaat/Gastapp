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
        [ObservableProperty] private float _totalAmount = 0;
        [ObservableProperty] private CalendarSelectionMode _selectionMode = CalendarSelectionMode.Single;
        [ObservableProperty] private MonthTemplate _template;
        [ObservableProperty] private DateTime _startDateRange = DateTime.UnixEpoch;
        [ObservableProperty] private DateTime _endDateRange = DateTime.UnixEpoch;
        [ObservableProperty] private DateTime _maxDate = DateTime.Now;
        [ObservableProperty] private DateTime _minDate = DateTime.Now;
        [ObservableProperty] private DateTime _selectedDate = DateTime.Now;
        [ObservableProperty] ObservableCollection<Spending> _spendingList = new();
        [ObservableProperty] ObservableCollection<DateTime> _datesWithSpending = new();

        public TodaySpendingsViewModel(SpendingService spendingService)
        {
            _spendingService = spendingService;
            GetData();
            Template = new MonthTemplate();
        }

        partial void OnSpendingListChanged(ObservableCollection<Spending> value)
        {
            if (value.Count > 0)
            {
                TotalAmount = value.Sum(x => (float)x.Amount);
            }
        }

        [RelayCommand]
        private void ChangeSelectionMode()
        {
            SelectionMode = SelectionMode == CalendarSelectionMode.Single
                ? CalendarSelectionMode.Range
                : CalendarSelectionMode.Single;
            if (SelectionMode == CalendarSelectionMode.Single)
                GetSpendingByDate(SelectedDate);
            else
                GetSpendingByDateRange(_startDateRange, _endDateRange);
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

            if (SpendingList.Count > 0)
            {
                MaxDate = DatesWithSpending.Max();
                MinDate = DatesWithSpending.Min();
            }

            if (DateTime.Now > MaxDate)
                MaxDate = DateTime.Now;

            if (DateTime.Now < MinDate)
                MinDate = DateTime.Now;
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