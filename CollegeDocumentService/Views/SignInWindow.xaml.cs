using ColledgeDocument.Shared.Requests;
using ColledgeDocument.Shared.Responses;
using CollegeDocumentService.Services;
using CollegeDocumentService.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mail;
using System.Text.Json;
using System.Windows;

namespace CollegeDocumentService;

public partial class SignInWindow : Window
{
    private readonly ITokenService _tokenService;
    private readonly IStorageService _storageService;

    public SignInWindow()
    {
        InitializeComponent();
        _tokenService = App.Host!.Services.GetRequiredService<ITokenService>();
        _storageService = App.Host!.Services.GetRequiredService<IStorageService>();
    }

    private async void GoToDocumentWindow(object sender, RoutedEventArgs e)
    {
        var username = UsernameTB.Text;
        var password = PasswordTB.Text;

        var request = new TokenRequest
        {
            Username = username,
            Password = password
        };

        var response = await _tokenService.SignInAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            MessageBox.Show(responseContent, "Ошибка");
            return;
        }

        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);
        _storageService.Set("AccessToken", tokenResponse!.AccessToken);
        _storageService.Set("RefreshToken", tokenResponse!.RefreshToken);

        var documentWindow = new DocumentWindow();
        documentWindow.Show();
        Close();
    }
}