using System.Windows;
using WpfHR.Views;

namespace WpfHR
{
    public partial class MainWindow : Window
    {
        private WpfHR.ViewModels.ModuleViewModel _moduleViewModel;

        public MainWindow()
        {
            InitializeComponent();
            _moduleViewModel = new WpfHR.ViewModels.ModuleViewModel();
        }

        private void OnAdaptationModulesClick(object sender, RoutedEventArgs e)
        {
            WelcomeText.Visibility = Visibility.Collapsed;
            ContentArea.Content = new ModuleView();
        }

        private void OnConstructorClick(object sender, RoutedEventArgs e)
        {
            WelcomeText.Visibility = Visibility.Collapsed;
            ContentArea.Content = new ConstructorView();
        }

        private void OnAnalysisClick(object sender, RoutedEventArgs e)
        {
            WelcomeText.Visibility = Visibility.Collapsed;
            ContentArea.Content = new AnalysisControl();
        }
    }
}
