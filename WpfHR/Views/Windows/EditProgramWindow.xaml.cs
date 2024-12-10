using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WpfHR.Helpers;
using WpfHR.Models;

namespace WpfHR.Views.Windows
{
    public partial class EditProgramWindow : Window
    {
        public ICommand SaveCommand { get; }
        public AdaptationProgram Program { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public string EmployeeName
        {
            get { return Program.EmployeeName; }
            set
            {
                Program.EmployeeName = value;
                OnPropertyChanged(nameof(EmployeeName));
            }
        }

        public string Position
        {
            get { return Program.Position; }
            set
            {
                Program.Position = value;
                OnPropertyChanged(nameof(Position));
            }
        }

        public int ErrorsCount
        {
            get { return Program.ErrorsCount; }
            set
            {
                Program.ErrorsCount = value;
                OnPropertyChanged(nameof(ErrorsCount));
            }
        }

        public double CompletionPercentage
        {
            get { return Program.CompletionPercentage; }
            set
            {
                Program.CompletionPercentage = (int)value;
                OnPropertyChanged(nameof(CompletionPercentage));
            }
        }

        public bool IsEmployed
        {
            get { return Program.IsEmployed; }
            set
            {
                Program.IsEmployed = value;
                OnPropertyChanged(nameof(IsEmployed));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public EditProgramWindow(AdaptationProgram program)
        {
            InitializeComponent();
            Program = program;
            DataContext = this;
            SaveCommand = new RelayCommand(SaveProgram);
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        private void SaveProgram()
        {
            using (var context = new ModuleDbContext())
            {
                context.AdaptationPrograms.Update(Program);
                context.SaveChanges();
            }

            DialogResult = true;
            Close();
        }
    }
}
