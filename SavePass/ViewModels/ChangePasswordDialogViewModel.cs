using System.ComponentModel.DataAnnotations;
using SavePass.DialogService;

namespace SavePass.ViewModels
{
    public class ChangePasswordDialogViewModel : ConfirmationViewModel<Confirmation<string>>
    {
        private string newPassword;
        private string repeatPassword;

        public string NewPassword
        {
            get => newPassword;
            set
            {
                SetProperty(ref newPassword, value);
                RaisePropertyChanged(nameof(RepeatPassword));
                ApplyCommand.RaiseCanExecuteChanged();
            }
        }

        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match")]
        public string RepeatPassword
        {
            get => repeatPassword;
            set
            {
                SetProperty(ref repeatPassword, value);
                ApplyCommand.RaiseCanExecuteChanged();
            }
        }

        protected override void OnApplyCommand()
        {
            Parameter.Value = NewPassword;
            base.OnApplyCommand();
        }

        protected override bool OnCanExecuteApplyCommand() => NewPassword == RepeatPassword;
    }
}