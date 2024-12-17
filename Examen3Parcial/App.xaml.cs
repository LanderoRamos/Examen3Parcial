namespace Examen3Parcial
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Views.ABMenu());
        }
    }
}
