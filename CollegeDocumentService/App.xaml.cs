using CollegeDocumentService.Handlers;
using CollegeDocumentService.Navigators;
using CollegeDocumentService.Options;
using CollegeDocumentService.Services;
using CollegeDocumentService.ViewModels;
using CollegeDocumentService.ViewModels.Base;
using CollegeDocumentService.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Windows;

namespace CollegeDocumentService
{
    public partial class App : Application
    {
        private readonly IHost? Host;
        public App()
        {
            Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .AddEnvironmentVariables();
                })
                .ConfigureServices(ConfigureServices)
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await Host!.StartAsync();

            var mainWindow = Host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await Host!.StopAsync();
            base.OnExit(e);
        }

        private void ConfigureServices(HostBuilderContext context,
            IServiceCollection services)
        {
            var configuration = context.Configuration;

            services.Configure<HttpClientOptions>(configuration.GetSection(nameof(HttpClientOptions)));

            var httpClientOptions = configuration.GetSection(nameof(HttpClientOptions)).Get<HttpClientOptions>() ?? throw new ArgumentNullException();

            services
                .AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                  .CreateClient(httpClientOptions.ClientName));

            services
                .AddSingleton<Func<Type, ViewModel>>(serviceProvider =>
                    viewModelType => (ViewModel)serviceProvider.GetRequiredService(viewModelType));

            services
                .AddSingleton(provider => new MainWindow()
                {
                    DataContext = provider.GetRequiredService<MainWindowVM>()
                })
                .AddTransient<SignInView>();

            services
                .AddSingleton<IMainWindowNavigator, MainWindowNavigator>();

            services
                .AddSingleton<MainWindowVM>()
                .AddTransient<SignInVM>();

            services
                .AddTransient<AuthorizationHandler>()
                .AddHttpClient(httpClientOptions.ClientName)
                .ConfigureHttpClient(client => client.BaseAddress = new Uri(httpClientOptions.ApiEndPoint))
                .AddHttpMessageHandler<AuthorizationHandler>();

            services
                .AddTransient<ITokenService, TokenService>()
                .AddTransient<IStorageService, StorageService>();
        }
    }

}
