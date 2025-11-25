namespace EkandidatoApp

{

    public partial class App : Application

    {

        public App()

        {
            InitializeComponent();


            InitializeComponent();

            MainPage = new AppShell();

        }
        protected override async void OnStart()

        {

            await Shell.Current.GoToAsync($"//LoginPage");

        }

    }

}