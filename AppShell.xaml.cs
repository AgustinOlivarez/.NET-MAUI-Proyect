namespace MauiApp1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.PlayerDetailPage), typeof(Views.PlayerDetailPage));
            Routing.RegisterRoute(nameof(Views.AddPlayerPage), typeof(Views.AddPlayerPage));
        }
    }
}
