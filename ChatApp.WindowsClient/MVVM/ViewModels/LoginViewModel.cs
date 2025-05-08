using System.ComponentModel.DataAnnotations;
using System.Windows;
using ChatApp.WindowsClient.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChatApp.WindowsClient.MVVM.ViewModels
{
    public partial class LoginViewModel : ObservableValidator
    {
        private INavigationService _navigation;

        [ObservableProperty]
        [Required(ErrorMessage = "Username is required")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters")]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string username = string.Empty;

        [ObservableProperty]
        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string password = string.Empty;

        public LoginViewModel(INavigationService navigationService)
        {
            _navigation = navigationService;
        }

        public void ValidatePassword()
        {
            ValidateProperty(Password, nameof(Password));
        }

        [RelayCommand(CanExecute = nameof(CanLogin))]
        private void Login()
        {
            ValidateAllProperties();

            if (HasErrors)
                return;

            MessageBox.Show($"Logged in as {Username}");
        }

        [RelayCommand]
        private void GoToRegister()
        {
            _navigation.NavigateTo<RegisterViewModel>();
        }

        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password);
        }
    }
}
