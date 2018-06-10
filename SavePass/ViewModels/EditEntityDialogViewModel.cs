using System;
using System.ComponentModel.DataAnnotations;
using Zabrodin.SavePass.DialogService;
using Zabrodin.SavePass.Models;

namespace Zabrodin.SavePass.ViewModels
{
    public class EditEntityDialogViewModel : ConfirmationViewModel<Confirmation<SavePassItem>>
    {
        private string name;
        private string userName;
        private string password;
        private string email;
        private string webSite;
        private string notes;

        [Required(ErrorMessage = "Field is required")]
        public string Name
        {
            get => name;
            set
            {
                SetProperty(ref name, value);
                ApplyCommand.RaiseCanExecuteChanged();
            }
        }

        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string WebSite
        {
            get => webSite;
            set => SetProperty(ref webSite, value);
        }

        public string Notes
        {
            get => notes;
            set => SetProperty(ref notes, value);
        }

        public override void Initialize(Confirmation<SavePassItem> param)
        {
            base.Initialize(param);

            Name = Parameter.Value.Name;
            UserName = Parameter.Value.UserName;
            Password = Parameter.Value.Password;
            Email = Parameter.Value.Email;
            WebSite = Parameter.Value.WebSite;
            Notes = Parameter.Value.Notes;
        }

        protected override bool OnCanExecuteApplyCommand() => !String.IsNullOrWhiteSpace(Name);

        protected override void OnApplyCommand()
        {
            Parameter.Value.Name = Name;
            Parameter.Value.UserName = UserName;
            Parameter.Value.Password = Password;
            Parameter.Value.Email = Email;
            Parameter.Value.WebSite = WebSite;
            Parameter.Value.Notes = Notes;

            base.OnApplyCommand();
        }
    }
}