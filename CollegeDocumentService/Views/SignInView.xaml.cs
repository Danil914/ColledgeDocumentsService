using ColledgeDocument.Shared.Requests;
using ColledgeDocument.Shared.Responses;
using CollegeDocumentService.Services;
using CollegeDocumentService.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mail;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace CollegeDocumentService.Views;

public partial class SignInView : UserControl
{

    public SignInView()
    {
        InitializeComponent();

    }

}