using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.Win32;
using Prism.Commands;
using Zabrodin.SavePass.DialogService;
using Zabrodin.SavePass.Validation;
using Zabrodin.SavePass.ViewModels.Contexts;

namespace Zabrodin.SavePass.ViewModels
{
    public class OpenFileDialogViewModel : ConfirmationViewModel<OpenFileContext>
    {
        [Required(ErrorMessage = "Field is required")]
        public string NewFilePath
        {
            get => GetProperty(() => NewFilePath);
            set => SetProperty(() => NewFilePath, value);
        }

        [FileExistsValidation(ErrorMessage = "File is not exists")]
        public string OpenFilePath
        {
            get => GetProperty(() => OpenFilePath);
            set => SetProperty(() => OpenFilePath, value);
        }

        public DelegateCommand BrowseCommand
        {
            get => GetProperty(() => BrowseCommand);
            private set => SetProperty(() => BrowseCommand, value);
        }

        public override void Initialize(OpenFileContext param)
        {
            base.Initialize(param);

            NewFilePath = OpenFilePath = Parameter.FilePath;
            BrowseCommand = new DelegateCommand(OnBrowseCommand);
        }

        protected override bool OnCanExecuteApplyCommand() => Parameter.IsSave || File.Exists(GetFilePath());

        protected override void OnApplyCommand()
        {
            Parameter.FilePath = GetFilePath();
            base.OnApplyCommand();
        }

        private void OnBrowseCommand()
        {
            FileDialog dialog = Parameter.IsSave
                ? (FileDialog) new SaveFileDialog
                {
                    Title = Parameter.Title,
                    OverwritePrompt = true,
                    Filter = "SavePass file (*.savepass)|*.savepass|All (*.*)|*.*"
                }
                : new OpenFileDialog
                {
                    Title = Parameter.Title,
                    CheckFileExists = true,
                    Filter = "SavePass file (*.savepass)|*.savepass|All (*.*)|*.*"
                };

            if (dialog.ShowDialog() != true)
                return;

            NewFilePath = OpenFilePath = dialog.FileName;
            ApplyCommand.RaiseCanExecuteChanged();
        }

        private string GetFilePath() => (Parameter.IsSave ? NewFilePath : OpenFilePath) ?? String.Empty;
    }
}