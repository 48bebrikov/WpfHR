using System.Windows;
using System.Windows.Controls;

namespace WpfHR
{
    public partial class MainWindow : Window
    {
        private WpfHR.ViewModels.ModuleViewModel _moduleViewModel;

        public MainWindow()
        {
            InitializeComponent();
            _moduleViewModel = new WpfHR.ViewModels.ModuleViewModel(); // Создаем один экземпляр
        }

        private void OnAdaptationModulesClick(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new WpfHR.Views.ModuleView();
        }

        private void OnConstructorClick(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new WpfHR.Views.ConstructorView();
        }

        private void OnAnalysisClick(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new WpfHR.Views.AnalysisControl();
        }
    }
}
