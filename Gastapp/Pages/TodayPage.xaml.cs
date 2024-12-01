using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using Gastapp.BottomSheets;
using Gastapp.Services.Spending;
using Gastapp.ViewModels.Today;
using Microsoft.Maui.Layouts;
using The49.Maui.BottomSheet;

namespace Gastapp.Pages;

public partial class TodayPage : ContentPage
{
    private readonly TodayViewModel _vm;
    private readonly NewSpendingBottomSheet _bs;
    private bool _isBsOpen = false;

    public TodayPage(TodayViewModel vm, NewSpendingBottomSheet bs, SpendingService spendingService)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
        _vm.todayPage = this;
        _bs = bs;
        _bs = new(vm);
    }


    protected override bool OnBackButtonPressed()
    {
        if (_vm.IsBsOpen)
        {
            _ = _bs.DismissAsync();
            _vm.IsBsOpen = false;
            return true;
        }

        return false;
    }

    protected override void OnAppearing()
    {
        _vm.GetTodayAmount();
        base.OnAppearing();
    }


    private void OpenBottomSheet(object? sender, EventArgs e)
    {
        if (!_vm.IsBsOpen)
        {
            _ = _bs.ShowAsync();
            _vm.IsBsOpen = true;
        }
    }
}