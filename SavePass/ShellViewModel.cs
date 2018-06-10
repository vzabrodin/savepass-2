using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Zabrodin.SavePass
{
    public class ShellViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private DelegateCommand<Window> minimizeWindowCommand;
        private DelegateCommand<Window> maximizeWindowCommand;
        private DelegateCommand<Window> closeWindowCommand;

        public ShellViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public DelegateCommand<Window> MinimizeWindowCommand
        {
            get => minimizeWindowCommand;
            private set => SetProperty(ref minimizeWindowCommand, value);
        }

        public DelegateCommand<Window> MaximizeWindowCommand
        {
            get => maximizeWindowCommand;
            private set => SetProperty(ref maximizeWindowCommand, value);
        }

        public DelegateCommand<Window> CloseWindowCommand
        {
            get => closeWindowCommand;
            private set => SetProperty(ref closeWindowCommand, value);
        }

        public void OnShellLoaded()
        {
            MinimizeWindowCommand = new DelegateCommand<Window>(MinimizeWindow);
            MaximizeWindowCommand = new DelegateCommand<Window>(MaximizeWindow);
            CloseWindowCommand = new DelegateCommand<Window>(CloseWindow);

            regionManager.RegisterViewWithRegion(ViewNames.MainView, typeof(Views.MainView));
        }

        private void MinimizeWindow(Window window) => SystemCommands.MinimizeWindow(window);

        private void MaximizeWindow(Window window)
        {
            if (window.WindowState == WindowState.Maximized)
                SystemCommands.RestoreWindow(window);
            else
                SystemCommands.MaximizeWindow(window);
        }

        private void CloseWindow(Window window) => SystemCommands.CloseWindow(window);
    }
}
