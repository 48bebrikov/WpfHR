using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfHR.Models;
using WpfHR.Views.Windows;
using WpfHR.Services;

namespace WpfHR.Views
{
    public partial class ConstructorView : UserControl
    {
        private readonly ModuleDbContext _context;
        private List<Employee> _mentorsCache;
        private readonly ProgramSaveService _programSaveService; // Создаем объект сервиса

        private readonly Employee _noSelectionEmployee = new Employee
        {
            Id = -1,
            FullName = "Нет выбора",
            Department = "",
            Position = ""
        };

        public ConstructorView()
        {
            InitializeComponent();
            _context = new ModuleDbContext();
            _programSaveService = new ProgramSaveService(); // Инициализация сервиса
            LoadData();

            EmployeeList.SelectionChanged += OnEmployeeSelectionChanged;
        }

        private void LoadData()
        {
            var employees = _context.Employees.ToList();
            employees.Insert(0, _noSelectionEmployee);
            EmployeeList.ItemsSource = employees;

            UpdateModulesForSelectedEmployee();

            var modules = _context.Modules.ToList();
            _mentorsCache = modules
                .SelectMany(m => m.Developers.Concat(m.Approvers))
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct()
                .Select(name => new Employee { Id = 0, FullName = name })
                .OrderBy(e => e.FullName)
                .ToList();

            MentorsList.ItemsSource = _mentorsCache;
        }

        private void OnEmployeeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateModulesForSelectedEmployee();
        }

        private void UpdateModulesForSelectedEmployee()
        {
            if (EmployeeList.SelectedItem is Employee selectedEmployee && selectedEmployee.Id != -1)
            {
                var modules = _context.Modules
                    .Where(m => m.Position == selectedEmployee.Position)
                    .ToList();

                ModulesPanel.ItemsSource = modules;
            }
            else
            {
                ModulesPanel.ItemsSource = _context.Modules.ToList();
            }
        }

        private void OnMentorSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = MentorSearchBox.Text.ToLower();
            MentorsList.ItemsSource = _mentorsCache
                .Where(m => m.FullName.ToLower().Contains(searchText))
                .ToList();
        }

        private void OnAddEmployeeClick(object sender, RoutedEventArgs e)
        {
            var addEmployeeWindow = new AddEmployeeWindow();
            addEmployeeWindow.ShowDialog();
            LoadData();
        }

        private void OnCreateProgramClick(object sender, RoutedEventArgs e)
        {
            if (EmployeeList.SelectedItem is not Employee employee || employee.Id == -1)
            {
                MessageBox.Show("Выберите сотрудника!");
                return;
            }

            var selectedModules = ModulesPanel.ItemsSource
                .OfType<Module>()
                .Where(m => m.IsChecked)
                .ToList();

            if (!selectedModules.Any())
            {
                MessageBox.Show("Выберите хотя бы один модуль!");
                return;
            }

            var selectedMentors = MentorsList.SelectedItems
                .OfType<Employee>()
                .Select(m => m.FullName)
                .ToList();

            if (!selectedMentors.Any())
            {
                MessageBox.Show("Выберите хотя бы одного наставника!");
                return;
            }

            var filePath = _programSaveService.GetSaveFilePath(employee); // Вызов метода сервиса для получения пути
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Сохранение отменено.");
                return;
            }

            try
            {
                _programSaveService.SaveProgramToFile(filePath, employee, selectedModules, selectedMentors); // Вызов метода сервиса для сохранения файла

                var program = new AdaptationProgram
                {
                    EmployeeId = employee.Id,
                    EmployeeName = employee.FullName,
                    Department = employee.Department,
                    Position = employee.Position,
                    SelectedModules = selectedModules,
                    Mentors = selectedMentors,
                    StartDate = DateTime.Now,
                    FilePath = filePath
                };

                _context.AdaptationPrograms.Add(program);
                _context.SaveChanges();

                MessageBox.Show("Программа адаптации успешно создана!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании программы: {ex.Message}");
            }
        }
    }
}
