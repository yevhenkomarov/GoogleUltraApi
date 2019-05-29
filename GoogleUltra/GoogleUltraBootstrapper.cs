using System.Windows;
using Caliburn.Micro;
using GoogleUltra.ViewModels;

namespace GoogleUltra
{
    public class GoogleUltraBootstrapper : BootstrapperBase
    {
        public GoogleUltraBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}