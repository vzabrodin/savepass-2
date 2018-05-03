using System;
using System.Windows.Threading;

namespace SavePass.Extensions
{
    public static class DispatcherExtensions
    {
        public static void ExecuteWithCheck(this Dispatcher dispatcher, Action method)
        {
            if (dispatcher.CheckAccess())
                method();
            else
                dispatcher.Invoke(method);
        }
    }
}
