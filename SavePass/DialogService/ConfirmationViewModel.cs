using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Zabrodin.SavePass.Validation;

namespace Zabrodin.SavePass.DialogService
{
    public abstract class ConfirmationViewModel<T> : ValidationBase
        where T : Confirmation
    {
        private T parameter;
        private DelegateCommand cancelCommand;
        private DelegateCommand applyCommand;

        public DelegateCommand ApplyCommand
        {
            get => applyCommand;
            protected set => SetProperty(ref applyCommand, value);
        }

        public DelegateCommand CancelCommand
        {
            get => cancelCommand;
            protected set => SetProperty(ref cancelCommand, value);
        }

        public T Parameter
        {
            get => parameter;
            set => SetProperty(ref parameter, value);
        }

        public virtual void Initialize(T param)
        {
            Parameter = param;

            ApplyCommand = new DelegateCommand(OnApplyCommand, OnCanExecuteApplyCommand);
            CancelCommand = new DelegateCommand(OnCancelCommand, OnCanExecuteCancelCommand);
        }

        public void Close() => DialogHost.CloseDialogCommand.Execute(null, null);

        protected virtual void OnCancelCommand()
        {
            Parameter.Confirmed = false;
            Close();
        }

        protected virtual bool OnCanExecuteCancelCommand() => true;

        protected virtual void OnApplyCommand()
        {
            Parameter.Confirmed = true;
            Close();
        }

        protected virtual bool OnCanExecuteApplyCommand() => true;
    }
}