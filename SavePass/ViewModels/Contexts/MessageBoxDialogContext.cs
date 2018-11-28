using System.Windows;
using Zabrodin.SavePass.DialogService;

namespace Zabrodin.SavePass.ViewModels.Contexts
{
    public class MessageBoxDialogContext : Confirmation
    {
        public string Message
        {
            get => GetProperty(() => Message);
            set => SetProperty(() => Message, value);
        }

        public MessageBoxButton Buttons
        {
            get => GetProperty(() => Buttons);
            set => SetProperty(() => Buttons, value);
        }

        public MessageBoxImage Image
        {
            get => GetProperty(() => Image);
            set => SetProperty(() => Image, value);
        }

        public MessageBoxResult Result
        {
            get => GetProperty(() => Result);
            set => SetProperty(() => Result, value);
        }
    }
}
