using System.Threading.Tasks;
using System.Windows;

namespace SavePass.DialogService
{
    public interface IDialogService
    {
        Task<T> Show<T>(string viewName, T context) where T : Confirmation;

        Task<MessageBoxResult> ShowMessageBox(string message, string title = null,
            MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxImage image = MessageBoxImage.Asterisk);
    }
}
