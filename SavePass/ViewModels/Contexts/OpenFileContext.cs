using Zabrodin.SavePass.DialogService;

namespace Zabrodin.SavePass.ViewModels.Contexts
{
    public class OpenFileContext : Confirmation
    {
        public string FilePath
        {
            get => GetProperty(() => FilePath);
            set => SetProperty(() => FilePath, value);
        }

        public string Password
        {
            get => GetProperty(() => Password);
            set => SetProperty(() => Password, value);
        }

        public bool IsSave
        {
            get => GetProperty(() => IsSave);
            set => SetProperty(() => IsSave, value);
        }

        public bool ShowPassword
        {
            get => GetProperty(() => ShowPassword);
            set => SetProperty(() => ShowPassword, value);
        }
    }
}