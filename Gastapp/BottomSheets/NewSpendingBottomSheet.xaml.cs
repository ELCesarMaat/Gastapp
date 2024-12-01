using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using Gastapp.Pages;
using Gastapp.Popups;
using Gastapp.Services.Spending;
using Gastapp.ViewModels.Today;
using The49.Maui.BottomSheet;

namespace Gastapp.BottomSheets;

public partial class NewSpendingBottomSheet : BottomSheet
{
    private readonly TodayViewModel _vm;
    public NewSpendingBottomSheet(TodayViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
        Showing += OnShowing;
        Dismissed += OnDismissed;
    }

    private void OnDismissed(object? sender, DismissOrigin e)
    {
        _vm.IsBsOpen = false;
        _vm.GetTodayAmount();
    }

    private void OnShowing(object? sender, EventArgs e)
    {
        _vm.GetCategories();
    }

    private async void SaveSpending(object? sender, EventArgs e)
    {
        if (await _vm.SaveSpending())
            await DismissAsync();
    }

    private async void CloseBottomSheet(object? sender, TappedEventArgs e)
    {
        _vm.ClearNewSpendingFields();
        await DismissAsync();
    }
    
}