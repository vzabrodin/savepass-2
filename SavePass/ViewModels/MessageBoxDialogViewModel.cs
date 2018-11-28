using System.Windows;
using Prism.Commands;
using Zabrodin.SavePass.DialogService;
using Zabrodin.SavePass.ViewModels.Contexts;

namespace Zabrodin.SavePass.ViewModels
{
    public class MessageBoxDialogViewModel : ConfirmationViewModel<MessageBoxDialogContext>
    {
        public DelegateCommand YesCommand
        {
            get => GetProperty(() => YesCommand);
            private set => SetProperty(() => YesCommand, value);
        }

        public DelegateCommand NoCommand
        {
            get => GetProperty(() => NoCommand);
            private set => SetProperty(() => NoCommand, value);
        }

        public DelegateCommand OKCommand
        {
            get => GetProperty(() => OKCommand);
            private set => SetProperty(() => OKCommand, value);
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
