using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Zabrodin.SavePass.Validation;

namespace Zabrodin.SavePass.DialogService
{
    public abstract class ConfirmationViewModel<T> : ValidationBase
        where T : Confirmation
    {
        public DelegateCommand ApplyCommand
        {
            get => GetProperty(() => ApplyCommand);
            protected set => SetProperty(() => ApplyCommand, value);
        }

        public DelegateCommand CancelCommand
        {
            get => GetProperty(() => CancelCommand);
            protected set => SetProperty(() => CancelCommand, value);
        }

        public T Parameter
        {
            get => GetProperty(() => Parameter);
            set => SetProperty(() => Parameter, value);
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