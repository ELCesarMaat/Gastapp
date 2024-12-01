using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gastapp.Data;
using Gastapp.Models;
using Gastapp.Services.Navigation;

namespace Gastapp.ViewModels.Welcome
{
    public partial class WelcomeViewModel(INavigationService navigationService, GastappDbContext dbContext) : ObservableObject
    {
        INavigationService _navigationService = navigationService;
        GastappDbContext _dbContext = dbContext;

        [ObservableProperty] private string _name;
        [ObservableProperty] private string _categoryName;
        [ObservableProperty] private bool _isButtonActive = false;

        [RelayCommand]
        private void SaveChanges()
        {
            Name = Name.Trim();
            CategoryName = CategoryName.Trim();
            var newCategory = new Category
            {
                CategoryName = CategoryName,
                IsInitial = true,
            };
            Preferences.Set("username", Name);

            _dbContext.Categories.Add(newCategory);
            _dbContext.SaveChanges();
            navigationService.NavigateToAsync($"//TodayPage");
        }

        private void ValidateFields()
        {
            var result =  !string.IsNullOrWhiteSpace(Name)
                   && !string.IsNullOrWhiteSpace(CategoryName);

            IsButtonActive = result;
        }

        partial void OnCategoryNameChanged(string value)
        {
            ValidateFields();
        }

        partial void OnNameChanged(string value)
        {
            ValidateFields();
        }
    }
}
