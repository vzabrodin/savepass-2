using Zabrodin.SavePass.DialogService;

namespace Zabrodin.SavePass.ViewModels.Contexts
{
    public class OpenFileContext : Confirmation
    {
        private string filePath;
        private string password;
        private bool isSave;
        private bool showPassword;

        public string FilePath
        {
            get => filePath;
            set => SetProperty(ref filePath, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public bool IsSave
        {
            get => isSave;
            set => SetProperty(ref isSave, value);
        }

        public bool ShowPassword
        {
            get => showPassword;
            set => SetProperty(ref showPassword, value);
        }
    }
}