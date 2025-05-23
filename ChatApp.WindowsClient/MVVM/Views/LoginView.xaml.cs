﻿using System.Windows;
using System.Windows.Controls;
using ChatApp.WindowsClient.MVVM.ViewModels;

namespace ChatApp.WindowsClient.MVVM.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = ((PasswordBox)sender).Password;
                vm.ValidatePassword();
            }
        }
    }
}
