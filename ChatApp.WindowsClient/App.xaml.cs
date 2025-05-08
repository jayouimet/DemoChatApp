using System.Windows;
using ChatApp.WindowsClient.MVVM.ViewModels;
using ChatApp.WindowsClient.MVVM.Views;
using ChatApp.WindowsClient.Services;
using ChatApp.WindowsClient.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.WindowsClient;

public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;

    public App()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton<MainWindow>(provider => new MainWindow
        {
            DataContext = provider.GetRequiredService<MainViewModel>()
        });
        services.AddSingleton<MainViewModel>();

        services.AddSingleton<MainLoadingView>(provider => new MainLoadingView
        {
            DataContext = provider.GetRequiredService<MainLoadingViewModel>()
        });
        services.AddSingleton<MainLoadingViewModel>();

        services.AddSingleton<LoginView>(provider => new LoginView
        {
            DataContext = provider.GetRequiredService<LoginViewModel>()
        });
        services.AddSingleton<LoginViewModel>();

        services.AddSingleton<RegisterView>(provider => new RegisterView
        {
            DataContext = provider.GetRequiredService<RegisterViewModel>()
        });
        services.AddSingleton<RegisterViewModel>();

        services.AddSingleton<INavigationService, NavigationService>();

        services.AddSingleton<Func<Type, ObservableObject>>(serviceProvider => viewModelType => (ObservableObject)serviceProvider.GetRequiredService(viewModelType));

        _serviceProvider = services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var window = _serviceProvider.GetRequiredService<MainWindow>();
        window.Show();
        base.OnStartup(e);
    }
}