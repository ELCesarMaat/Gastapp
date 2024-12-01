using Gastapp.ViewModels.Welcome;

namespace Gastapp.Pages;

public partial class Welcome : ContentPage
{
	public Welcome(WelcomeViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}