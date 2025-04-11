using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Gastapp.ViewModels.Register
{
    public partial class RegisterViewModel : ObservableObject
    {
        [ObservableProperty] private string _email;
        [ObservableProperty] private string _password;

        public RegisterViewModel()
        {

        }
    }
}
