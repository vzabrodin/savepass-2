using System.Windows;
using Prism.Commands;
using Zabrodin.SavePass.DialogService;
using Zabrodin.SavePass.ViewModels.Contexts;

namespace Zabrodin.SavePass.ViewModels
{
    public class MessageBoxDialogViewModel : ConfirmationViewModel<MessageBoxDialogContext>
    {
        public MessageBoxDialogViewModel()
        {
            YesCommand = new DelegateCommand(OnYesCommand);
            NoCommand = new DelegateCommand(OnNoCommand);
            OKCommand = new DelegateCommand(OnOKCommand);
        }

        public DelegateCommand YesCommand { get; }

        public DelegateCommand NoCommand { get; }

        public DelegateCommand OKCommand { get; }

        private void OnYesCommand()
        {
            Parameter.Result = MessageBoxResult.Yes;
            Close();
        }

        private void OnNoCommand()
        {
            Parameter.Result = MessageBoxResult.No;
            Close();
        }

        private void OnOKCommand()
        {
            Parameter.Result = MessageBoxResult.OK;
            Close();
        }

        protected override void OnCancelCommand()
        {
            Parameter.Result = MessageBoxResult.Cancel;
            Close();
        }
    }
}
