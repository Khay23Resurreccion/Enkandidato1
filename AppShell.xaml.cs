namespace EkandidatoApp
{
    public partial class AppShell : Shell
    {
        private bool _hasNavigated;
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(ForgotPage), typeof(ForgotPage));
            Routing.RegisterRoute(nameof(CreatePassword), typeof(CreatePassword));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        }

        }
    }

