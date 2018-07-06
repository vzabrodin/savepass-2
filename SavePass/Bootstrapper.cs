using System.Windows;
using Microsoft.Practices.Unity;
using Prism.Mvvm;
using Prism.Unity;
using Zabrodin.SavePass.DialogService;
using Zabrodin.SavePass.Views;

namespace Zabrodin.SavePass
{
    public class Bootstrapper : UnityBootstrapper
    {
        private readonly string[] args;

        public Bootstrapper(string[] args)
        {
            this.args = args;
        }

        // ReSharper disable once PossibleNullReferenceException
        protected override void InitializeShell() => Application.Current.MainWindow.Show();

        protected override DependencyObject CreateShell() => Container.Resolve<Shell>();

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            RegisterInstances();
            RegisterServices();
            RegisterViews();
        }

        private void RegisterInstances()
        {
            Container.RegisterInstance(InstanceNames.CommandLineArgs, args);
        }

        private void RegisterServices()
        {
            Container.RegisterType<IDialogService, DialogService.DialogService>();
        }

        private void RegisterViews()
        {
            Container.RegisterType<IDialogView, MessageBoxDialogView>(ViewNames.MessageBoxDialogView);
            Container.RegisterType<IDialogView, EditEntityDialogView>(ViewNames.EditEntityDialogView);
            Container.RegisterType<IDialogView, ChangePasswordDialogView>(ViewNames.ChangePasswordDialogView);
            Container.RegisterType<IDialogView, OpenFileDialogView>(ViewNames.OpenFileDialogView);
            ViewModelLocationProvider.Register(typeof(Shell).FullName, () => Container.Resolve<ShellViewModel>());
        }
    }
}
