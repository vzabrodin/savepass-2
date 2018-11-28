using Zabrodin.SavePass.Validation;

namespace Zabrodin.SavePass.DialogService
{
    public class Confirmation : ValidationBase
    {
        public bool Confirmed
        {
            get => GetProperty(() => Confirmed);
            set => SetProperty(() => Confirmed, value);
        }

        public string Title
        {
            get => GetProperty(() => Title);
            set => SetProperty(() => Title, value);
        }
    }

    public class Confirmation<T> : Confirmation
    {
        public Confirmation()
        {
        }

        public Confirmation(T value)
        {
            Value = value;
        }

        public T Value
        {
            get => GetProperty(() => Value);
            set => SetProperty(() => Value, value);
        }
    }
}