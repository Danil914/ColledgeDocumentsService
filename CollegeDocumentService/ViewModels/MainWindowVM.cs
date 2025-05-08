using CollegeDocumentService.Navigators;
using CollegeDocumentService.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CollegeDocumentService.ViewModels
{
    public partial class MainWindowVM : ViewModel
    {
        [ObservableProperty]
        private IMainWindowNavigator _mainWindowNavigator;

        public MainWindowVM(IMainWindowNavigator mainWindowNavigator)
        {
            MainWindowNavigator = mainWindowNavigator;
            MainWindowNavigator.NavigateTo<SignInVM>();
        }
    }
}
