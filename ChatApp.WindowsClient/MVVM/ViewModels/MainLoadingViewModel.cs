using ChatApp.WindowsClient.Services;
using ChatApp.WindowsClient.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ChatApp.WindowsClient.MVVM.ViewModels
{
    public partial class MainLoadingViewModel : ObservableObject
    {
        private INavigationService _navigation;

        public MainLoadingViewModel(INavigationService navigationService)
        {
            _navigation = navigationService;
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            await Task.Delay(2000);
            _navigation.NavigateTo<LoginViewModel>();
        }
    }
}
