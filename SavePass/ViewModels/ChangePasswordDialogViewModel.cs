using System.ComponentModel.DataAnnotations;
using Zabrodin.SavePass.DialogService;

namespace Zabrodin.SavePass.ViewModels
{
    public class ChangePasswordDialogViewModel : ConfirmationViewModel<Confirmation<string>>
    {
        public string NewPassword
        {
            get => GetProperty(() => NewPassword);
            set
            {
                SetProperty(() => NewPassword, value);
                RaisePropertyChanged(() => RepeatPassword);
            }
        }

        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match")]
        public string RepeatPassword
        {
            get => GetProperty(() => RepeatPassword);
            set => SetProperty(() => RepeatPassword, value);
        }

        protected override void OnApplyCommand()
        {
            Parameter.Value = NewPassword;
            base.OnApplyCommand();
        }

        protected override bool OnCanExecuteApplyCommand() => NewPassword == RepeatPassword;
    }
}