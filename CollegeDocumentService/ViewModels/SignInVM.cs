using ColledgeDocument.Shared.Requests;
using ColledgeDocument.Shared.Responses;
using CollegeDocumentService.Navigators;
using CollegeDocumentService.Services;
using CollegeDocumentService.ViewModels.Base;
using CollegeDocumentService.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace CollegeDocumentService.ViewModels
{
    public partial class SignInVM : ViewModel
    {
        private readonly ITokenService _tokenService;
        private readonly IStorageService _storageService;

        public SignInVM(
            ITokenService tokenService, 
            IStorageService storageService,
            IMainWindowNavigator mainWindowNavigator)
        {
            _tokenService = tokenService;
            _storageService =  storageService;
            MainWindowNavigator = mainWindowNavigator;
        }

        [ObservableProperty]
        private IMainWindowNavigator _mainWindowNavigator;

        [ObservableProperty]
        private TokenRequest _tokenRequest = new TokenRequest();

        [RelayCommand]
        private async Task GoToDocumentsWindowAsync()
        {
            var response = await _tokenService.SignInAsync(TokenRequest);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show(responseContent, "Ошибка");
                return;
            }

            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);
            _storageService.Set("AccessToken", tokenResponse!.AccessToken);
            _storageService.Set("RefreshToken", tokenResponse!.RefreshToken);

            await MainWindowNavigator.NavigateTo<DocumentsVM>();
        }

    }
}
