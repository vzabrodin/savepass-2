using System.Windows;
using Prism.Commands;
using Zabrodin.SavePass.DialogService;
using Zabrodin.SavePass.ViewModels.Contexts;

namespace Zabrodin.SavePass.ViewModels
{
    public class MessageBoxDialogViewModel : ConfirmationViewModel<MessageBoxDialogContext>
    {
        private DelegateCommand yesCommand;
        private DelegateCommand noCommand;
        private DelegateCommand okCommand;

        public DelegateCommand YesCommand
        {
            get => yesCommand;
            private set => SetProperty(ref yesCommand, value);
        }

        public DelegateCommand NoCommand
        {
            get => noCommand;
            private set => SetProperty(ref noCommand, value);
        }

        public DelegateCommand OKCommand
        {
            get => okCommand;
            private set => SetProperty(ref okCommand, value);
        }

        public override void Initialize(MessageBoxDialogContext param)
        {
            base.Initialize(param);

            YesCommand = new DelegateCommand(OnYesCommand);
            NoCommand = new DelegateCommand(OnNoCommand);
            OKCommand = new DelegateCommand(OnOKCommand);
        }

        protected override void OnCancelCommand()
        {
            Parameter.Result = MessageBoxResult.Cancel;
            Close();
        }

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
    }
}
