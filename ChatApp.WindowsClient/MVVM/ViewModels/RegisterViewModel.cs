using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Collections;
using ChatApp.WindowsClient.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChatApp.WindowsClient.MVVM.ViewModels
{
    public partial class RegisterViewModel : ObservableValidator
    {
        private INavigationService _navigation;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Username is required")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters")]
        private string username = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email format is invalid.")]
        private string email = string.Empty;

        [ObservableProperty]
        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string password = string.Empty;

        [ObservableProperty]
        [Required(ErrorMessage = "Password confirmation is required")]
        [MinLength(8, ErrorMessage = "Password confirmation must be at least 8 characters")]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        [CustomValidation(typeof(RegisterViewModel), nameof(ValidateConfirmPassword))]
        private string confirmPassword = string.Empty;

        public RegisterViewModel(INavigationService navigationService)
        {
            _navigation = navigationService;
        }

        [RelayCommand]
        private void GoToLogin()
        {
            _navigation.NavigateTo<LoginViewModel>();
        }

        [RelayCommand(CanExecute = nameof(CanRegister))]
        private void Register()
        {
            ValidateAllProperties();

            if (HasErrors)
                return;

            MessageBox.Show($"Registered as {Username}");
        }

        partial void OnPasswordChanged(string value)
        {
            ValidateProperty(value, nameof(Password));
            ValidateProperty(ConfirmPassword, nameof(ConfirmPassword));
        }

        partial void OnConfirmPasswordChanged(string value)
        {
            ValidateProperty(value, nameof(ConfirmPassword));
        }

        public static ValidationResult ValidateConfirmPassword(string confirmPassword, ValidationContext context)
        {
            var instance = (RegisterViewModel)context.ObjectInstance;

            if (string.IsNullOrWhiteSpace(confirmPassword) || string.IsNullOrWhiteSpace(instance.Password))
                return ValidationResult.Success!;

            return confirmPassword != instance.Password
                ? new ValidationResult("Passwords do not match.")
                : ValidationResult.Success!;
        }

        private bool CanRegister()
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !string.IsNullOrWhiteSpace(ConfirmPassword) &&
                   Password.Equals(ConfirmPassword);
        }
    }
}
