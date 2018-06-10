using System.Windows;
using Zabrodin.SavePass.DialogService;

namespace Zabrodin.SavePass.ViewModels.Contexts
{
    public class MessageBoxDialogContext : Confirmation
    {
        private string message;
        private MessageBoxButton buttons;
        private MessageBoxImage image;
        private MessageBoxResult result;

        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        public MessageBoxButton Buttons
        {
            get => buttons;
            set => SetProperty(ref buttons, value);
        }

        public MessageBoxImage Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }

        public MessageBoxResult Result
        {
            get => result;
            set => SetProperty(ref result, value);
        }
    }
}
