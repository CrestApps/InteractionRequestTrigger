using Prism.Unity;
using System.Windows;

namespace InteractionRequestTrigger
{
    public class Bootstrapper : UnityBootstrapper
    {

        protected override DependencyObject CreateShell()
        {
            return new Views.MainWindow();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window)this.Shell;
            Application.Current.MainWindow.Show();
        }
    }
}
