using System;
using System.IO;
using Microsoft.Win32;
using Prism.Commands;
using SavePass.DialogService;
using SavePass.ViewModels.Contexts;
using System.ComponentModel.DataAnnotations;
using SavePass.Validation;

namespace SavePass.ViewModels
{
    public class OpenFileDialogViewModel : ConfirmationViewModel<OpenFileContext>
    {
        private string newFilePath;
        private string openFilePath;

        public OpenFileDialogViewModel()
        {
            BrowseCommand = new DelegateCommand(OnBrowseCommand);
        }

        [Required(ErrorMessage = "Field is required")]
        public string NewFilePath
        {
            get => newFilePath;
            set
            {
                SetProperty(ref newFilePath, value);
                ApplyCommand.RaiseCanExecuteChanged();
            }
        }

        [FileExistsValidation(ErrorMessage = "File is not exists")]
        public string OpenFilePath
        {
            get => openFilePath;
            set
            {
                SetProperty(ref openFilePath, value);
                ApplyCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand BrowseCommand { get; }

        public override void Initialize(OpenFileContext param)
        {
            base.Initialize(param);

            NewFilePath = OpenFilePath = Parameter.FilePath;
        }

        protected override bool OnCanExecuteApplyCommand()
            => (Parameter?.IsSave ?? false) || File.Exists(GetFilePath());

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

            if (dialog.ShowDialog() == true)
            {
                NewFilePath = OpenFilePath = dialog.FileName;
                ApplyCommand.RaiseCanExecuteChanged();
            }
        }

        private string GetFilePath() => ((Parameter?.IsSave ?? false) ? NewFilePath : OpenFilePath)
                                        ?? String.Empty;
    }
}