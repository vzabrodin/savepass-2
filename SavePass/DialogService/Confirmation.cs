using SavePass.Validation;

namespace SavePass.DialogService
{
    public class Confirmation : ValidationBase
    {
        private bool confirmed;
        private string title;

        public bool Confirmed
        {
            get => confirmed;
            set => SetProperty(ref confirmed, value);
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
    }

    public class Confirmation<T> : Confirmation
    {
        private T value;

        public Confirmation()
        {
        }

        public Confirmation(T value)
        {
            Value = value;
        }

        public T Value
        {
            get => value;
            set => SetProperty(ref this.value, value);
        }
    }
}