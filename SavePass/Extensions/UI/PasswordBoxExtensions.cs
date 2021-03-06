﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace Zabrodin.SavePass.Extensions.UI
{
    public static class PasswordBoxExtensions
    {
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached(
            "Password", typeof(string), typeof(PasswordBoxExtensions),
            new FrameworkPropertyMetadata(String.Empty, OnPasswordPropertyChanged));

        public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached(
            "Attach", typeof(bool), typeof(PasswordBoxExtensions), new PropertyMetadata(false, Attach));

        private static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached(
            "IsUpdating", typeof(bool), typeof(PasswordBoxExtensions));

        public static void SetAttach(DependencyObject dp, bool value) => dp.SetValue(AttachProperty, value);

        public static bool GetAttach(DependencyObject dp) => (bool) dp.GetValue(AttachProperty);

        public static string GetPassword(DependencyObject dp) => (string) dp.GetValue(PasswordProperty);

        public static void SetPassword(DependencyObject dp, string value) => dp.SetValue(PasswordProperty, value);

        private static bool GetIsUpdating(DependencyObject dp) => (bool) dp.GetValue(IsUpdatingProperty);

        private static void SetIsUpdating(DependencyObject dp, bool value) => dp.SetValue(IsUpdatingProperty, value);

        private static void OnPasswordPropertyChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is PasswordBox passwordBox))
                return;

            passwordBox.PasswordChanged -= PasswordChanged;

            if (!GetIsUpdating(passwordBox))
                passwordBox.Password = (string) e.NewValue;

            passwordBox.PasswordChanged += PasswordChanged;
        }

        private static void Attach(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is PasswordBox passwordBox))
                return;

            if ((bool) e.OldValue)
                passwordBox.PasswordChanged -= PasswordChanged;

            if ((bool) e.NewValue)
                passwordBox.PasswordChanged += PasswordChanged;
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!(sender is PasswordBox passwordBox))
                return;

            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }
    }
}