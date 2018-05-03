using Microsoft.Win32;
using Prism.Commands;
using SavePass.DialogService;
using SavePass.ViewModels.Contexts;

namespace SavePass.ViewModels
{
    public class OpenFileDialogViewModel : ConfirmationViewModel<OpenFileContext>
    {
        public OpenFileDialogViewModel()
        {
            BrowseCommand = new DelegateCommand(OnBrowseCommand);
        }

        public DelegateCommand BrowseCommand { get; }

        private void OnBrowseCommand()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = Parameter.Title,
                CheckFileExists = Parameter.CheckFileExists,
                Filter = "SavePass file (*.savepass)|*.savepass|All (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            Parameter.OpenFilePath = openFileDialog.FileName;
            Parameter.NewFilePath = openFileDialog.FileName;
        }
    }
}