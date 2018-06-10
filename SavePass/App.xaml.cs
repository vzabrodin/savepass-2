using System.Windows;

namespace Zabrodin.SavePass
{
    public partial class App
    {
        private Bootstrapper bootstrapper;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            bootstrapper = new Bootstrapper(e.Args);
            bootstrapper.Run();
        }
    }
}
