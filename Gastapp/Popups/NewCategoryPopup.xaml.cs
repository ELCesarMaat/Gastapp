using CommunityToolkit.Maui.Views;
using Gastapp.Services.Spending;
using Gastapp.ViewModels.Today;

namespace Gastapp.Popups;

public partial class NewCategoryPopup : Popup
{
    private readonly TodayViewModel _vm;
    private readonly SpendingService _spendingService;
	public NewCategoryPopup(TodayViewModel vm, SpendingService spendingService)
	{
		InitializeComponent();
        BindingContext = _vm = vm;
        _spendingService = spendingService;
    }

    private void SaveClick(object? sender, EventArgs e)
    {
        var categoryName = txtCategory.Text;
        if (string.IsNullOrWhiteSpace(categoryName))
            return;

        if (_spendingService.CreateNewCategory(txtCategory.Text))
        {
            Close();
            _vm.GetCategories(replaceNewCategory: true);
        }
        

    }

    private void Button_OnClicked(object? sender, EventArgs e)
    {
        Close();
    }
}