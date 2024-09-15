namespace MauiQRDeviceTemplate
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(BarcodePage), typeof(BarcodePage));
        }
    }
}
