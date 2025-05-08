using ChatApp.WindowsClient.Services;
using ChatApp.WindowsClient.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ChatApp.WindowsClient.MVVM.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private INavigationService navigation;

        public MainViewModel(INavigationService navigationService)
        {
            navigation = navigationService;
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            await Task.Delay(1000);
            Navigation.NavigateTo<MainLoadingViewModel>();
        }
    }
}
