using System;
using System.Linq;
using System.Windows;
using WpfHR.Models;

namespace WpfHR.Views.Windows
{
    public partial class AddEmployeeWindow : Window
    {
        private readonly ModuleDbContext _context;

        public AddEmployeeWindow()
        {
            InitializeComponent();
            _context = new ModuleDbContext();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            EmployeesListBox.ItemsSource = _context.Employees.ToList();
        }

        private void OnAddEmployeeClick(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(DepartmentTextBox.Text) ||
                string.IsNullOrWhiteSpace(PositionTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newEmployee = new Employee
            {
                FullName = FullNameTextBox.Text.Trim(),
                Department = DepartmentTextBox.Text.Trim(),
                Position = PositionTextBox.Text.Trim()
            };

            try
            {
                _context.Employees.Add(newEmployee);
                _context.SaveChanges();

                MessageBox.Show("Сотрудник успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadEmployees();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении сотрудника: {ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearInputFields()
        {
            FullNameTextBox.Clear();
            DepartmentTextBox.Clear();
            PositionTextBox.Clear();
        }

        private void OnDeleteEmployeeClick(object sender, RoutedEventArgs e)
        {
            if (EmployeesListBox.SelectedItem is not Employee selectedEmployee)
            {
                MessageBox.Show("Пожалуйста, выберите сотрудника для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Вы уверены, что хотите удалить сотрудника {selectedEmployee.FullName}?",
                                         "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Employees.Remove(selectedEmployee);
                    _context.SaveChanges();

                    MessageBox.Show("Сотрудник успешно удалён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadEmployees();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при удалении сотрудника: {ex.Message}",
                                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
