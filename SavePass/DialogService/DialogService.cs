using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;
using Microsoft.Practices.Unity;
using SavePass.ViewModels.Contexts;

namespace SavePass.DialogService
{
    public class DialogService : IDialogService
    {
        private readonly IUnityContainer unityContainer;

        public DialogService(IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;
        }

        public async Task<T> Show<T>(string viewName, T context)
            where T : Confirmation
        {
            IDialogView view = unityContainer.Resolve<IDialogView>(viewName);

            var viewModel = view.DataContext as ConfirmationViewModel<T>;
            viewModel?.Initialize(context);

            DialogHost.CloseDialogCommand.Execute(null, null);

            await DialogHost.Show(view);
            return context;
        }

        public async Task<MessageBoxResult> ShowMessageBox(string message, string title = null,
            MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxImage image = MessageBoxImage.Asterisk)
        {
            return (await Show(ViewNames.MessageBoxDialogView,
                new MessageBoxDialogContext
                {
                    Title = title,
                    Message = message,
                    Buttons = buttons,
                    Image = image
                })).Result;
        }
    }
}