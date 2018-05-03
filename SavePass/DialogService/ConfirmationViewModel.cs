using MaterialDesignThemes.Wpf;
using Prism.Commands;
using SavePass.Validation;

namespace SavePass.DialogService
{
    public abstract class ConfirmationViewModel<T> : ValidationBase
        where T : Confirmation
    {
        private T parameter;

        protected ConfirmationViewModel()
        {
            ApplyCommand = new DelegateCommand(OnApplyCommand, OnCanExecuteApplyCommand);
            CancelCommand = new DelegateCommand(OnCancelCommand, OnCanExecuteCancelCommand);
        }

        public DelegateCommand ApplyCommand { get; }

        public DelegateCommand CancelCommand { get; }

        public T Parameter
        {
            get => parameter;
            set => SetProperty(ref parameter, value);
        }

        public virtual void Initialize(T param)
        {
            Parameter = param;
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