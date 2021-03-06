﻿using System.Windows;

namespace Zabrodin.SavePass
{
    public partial class Shell
    {
        public Shell()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnLoaded;
            (DataContext as ShellViewModel)?.OnShellLoaded();
        }
    }
}
