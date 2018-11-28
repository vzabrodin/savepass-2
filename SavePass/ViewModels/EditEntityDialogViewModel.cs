using System;
using System.ComponentModel.DataAnnotations;
using Zabrodin.SavePass.DialogService;
using Zabrodin.SavePass.Models;

namespace Zabrodin.SavePass.ViewModels
{
    public class EditEntityDialogViewModel : ConfirmationViewModel<Confirmation<SavePassItem>>
    {
        [Required(ErrorMessage = "Field is required")]
        public string Name
        {
            get => GetProperty(() => Name);
            set => SetProperty(() => Name, value);
        }

        public string UserName
        {
            get => GetProperty(() => UserName);
            set => SetProperty(() => UserName, value);
        }

        public string Password
        {
            get => GetProperty(() => Password);
            set => SetProperty(() => Password, value);
        }

        public string Email
        {
            get => GetProperty(() => Email);
            set => SetProperty(() => Email, value);
        }

        public string WebSite
        {
            get => GetProperty(() => WebSite);
            set => SetProperty(() => WebSite, value);
        }

        public string Notes
        {
            get => GetProperty(() => Notes);
            set => SetProperty(() => Notes, value);
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