using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gastapp.Models;
using Gastapp.Pages;
using Gastapp.Popups;
using Gastapp.Services.Navigation;
using Gastapp.Services.Spending;
using Gastapp.Services.User;
using The49.Maui.BottomSheet;

namespace Gastapp.ViewModels.Today
{
    public partial class TodayViewModel : ObservableObject
    {
        public bool IsBsOpen = false;
        private readonly INavigationService _navigationService;
        private readonly SpendingService _spendingService;
        public TodayPage? todayPage = null;

        [ObservableProperty] private double _todayAmount;

        [ObservableProperty] private ObservableCollection<Category> _categoriesList = new();
        [ObservableProperty] private decimal _amount = 0;
        [ObservableProperty] private string _amountText = string.Empty;
        [ObservableProperty] private Category? _selectedCategory;
        [ObservableProperty] private string _title = string.Empty;
        [ObservableProperty] private string _exampleWord = "Gasolina";

        public TodayViewModel(INavigationService navigationService, SpendingService spendingService) 
        {
            _navigationService = navigationService;
            _spendingService = spendingService;
        }

        #region TodayPage
        public void GetTodayAmount()
        {
            TodayAmount = _spendingService.GetTodayAmountAsync();
        }

        #endregion

        #region NewSpendingBs

        partial void OnAmountTextChanged(string value)
        {
            Amount = decimal.TryParse(value, out decimal amountParse) ? amountParse : 0;
        }

        public async Task<bool> SaveSpending()
        {
            if (!ValidateFields())
                return false;

            var newSpending = new Spending
            {
                Amount = Amount,
                Title = Title,
                CategoryId = SelectedCategory!.CategoryId
            };
            if (!await _spendingService.CreateSpendingAsync(newSpending))
                return false;
            ClearNewSpendingFields();
            return true;
        }

        private bool ValidateFields()
        {
            return Amount > 0
                   && SelectedCategory != null
                   && !string.IsNullOrWhiteSpace(Title);
        }

        public void ClearNewSpendingFields()
        {
            AmountText = string.Empty;
            Amount = 0;
            Title = string.Empty;
            SelectedCategory = null;
        }

        [RelayCommand]
        private void ShowNewCategoryPopup()
        {
            todayPage!.ShowPopup(new NewCategoryPopup(this, _spendingService));
        }

        public async void GetCategories(bool replaceNewCategory = false)
        {
            CategoriesList = await _spendingService.GetMyCategoriesAsync();
            if(replaceNewCategory)
                SelectedCategory = CategoriesList.MaxBy(x => x.CategoryId);
        }


        #endregion


    }
}
