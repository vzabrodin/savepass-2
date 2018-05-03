using System.ComponentModel.DataAnnotations;
using SavePass.DialogService;
using SavePass.Validation;

namespace SavePass.ViewModels.Contexts
{
    public class OpenFileContext : Confirmation
    {
        private string newFilePath;
        private string openFilePath;
        private string password;
        private bool checkFileExists;

        [Required(ErrorMessage = "Field is required")]
        public string NewFilePath
        {
            get => newFilePath;
            set => SetProperty(ref newFilePath, value);
        }

        [FileExistsValidation(ErrorMessage = "File is not exists")]
        public string OpenFilePath
        {
            get => openFilePath;
            set => SetProperty(ref openFilePath, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public bool CheckFileExists
        {
            get => checkFileExists;
            set => SetProperty(ref checkFileExists, value);
        }
    }
}