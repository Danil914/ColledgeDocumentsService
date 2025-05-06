using CollegeDocumentService.Options;
using CollegeDocumentService.Services;
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
        public static IHost? Host;
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

            var mainWindow = new SignInWindow();
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
            var configaration = context.Configuration;

            services.Configure<HttpClientOptions>(configaration.GetSection(nameof(HttpClientOptions)));

            var httpClientOptions = configaration.GetSection(nameof(HttpClientOptions)).Get<HttpClientOptions>() ?? throw new ArgumentNullException();

            services
                .AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                  .CreateClient(httpClientOptions.ClientName));

            services
                //.AddTransient<AuthorizationHandler>()
                .AddHttpClient(httpClientOptions.ClientName)
                .ConfigureHttpClient(client => client.BaseAddress = new Uri(httpClientOptions.ApiEndPoint));
                //.AddHttpMessageHandler<AuthorizationHandler>();

            services
                .AddTransient<ITokenService, TokenService>()
                .AddTransient<IStorageService, StorageService>();
        }
    }

}
