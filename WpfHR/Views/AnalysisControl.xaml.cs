using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfHR.Helpers;
using WpfHR.Models;
using WpfHR.Views.Windows;

namespace WpfHR.Views
{
    public partial class AnalysisControl : UserControl
    {
        public AnalysisControl()
        {
            InitializeComponent();
            DataContext = this;
            EditCommand = new RelayCommand<AdaptationProgram>(OpenEditWindow);
            LoadData();
        }

        public List<AdaptationProgram> Programs { get; private set; }
        public SeriesCollection SeriesCollection { get; private set; }
        public Axis AxisX { get; private set; }
        public Axis AxisY { get; private set; }
        public ICommand EditCommand { get; }
        public List<string> Departments { get; set; }
        public List<string> Positions { get; set; }
        public string SelectedDepartment { get; set; }
        public string SelectedPosition { get; set; }

        private void LoadData()
        {
            try
            {
                Programs = GetPrograms();

                if (Programs != null && Programs.Any())
                {
                    ProgramsDataGrid.ItemsSource = Programs;
                    UpdateChart();

                    Departments = Programs.Select(p => p.Department).Distinct().ToList();
                    Positions = Programs.Select(p => p.Position).Distinct().ToList();

                    OnPropertyChanged(nameof(Departments));
                    OnPropertyChanged(nameof(Positions));
                }
                else
                {
                    MessageBox.Show("Нет данных для отображения программ адаптации.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }

        private List<AdaptationProgram> GetPrograms()
        {
            try
            {
                using var context = new ModuleDbContext();

                return context.AdaptationPrograms
                    .Where(p => !string.IsNullOrEmpty(p.EmployeeName) && !string.IsNullOrEmpty(p.Department) && !string.IsNullOrEmpty(p.Position))
                    .Include(p => p.Employee)
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении данных из базы: {ex.Message}");
                return new List<AdaptationProgram>();
            }
        }

        private void UpdateChart()
        {
            try
            {
                if (Programs == null || !Programs.Any())
                {
                    MessageBox.Show("Нет данных для построения диаграммы.");
                    return;
                }

                var chartData = Programs
                    .GroupBy(p => p.Department)
                    .Select(g => new
                    {
                        Category = g.Key,
                        AverageCompletion = g.Average(p => p.CompletionPercentage),
                        AverageErrors = g.Average(p => p.ErrorsCount),
                        EmploymentRate = g.Count(p => p.IsEmployed) * 100.0 / g.Count()
                    })
                    .ToList();

                // Создаем коллекцию серий для диаграммы
                SeriesCollection = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Среднее выполнение (%)",
                        Values = new ChartValues<double>(chartData.Select(d => d.AverageCompletion))
                    },
                    new ColumnSeries
                    {
                        Title = "Среднее количество ошибок",
                        Values = new ChartValues<double>(chartData.Select(d => d.AverageErrors))
                    },
                    new ColumnSeries
                    {
                        Title = "Процент трудоустройства (%)",
                        Values = new ChartValues<double>(chartData.Select(d => d.EmploymentRate))
                    }
                };

                AxisX = new Axis
                {
                    Title = "Отделы",
                    Labels = chartData.Select(d => d.Category).ToArray()
                };

                AxisY = new Axis
                {
                    Title = "Значения",
                    LabelFormatter = value => value.ToString("N0") 
                };

                if (completionChart.AxisX == null)
                {
                    completionChart.AxisX = new AxesCollection();
                }
                if (completionChart.AxisY == null)
                {
                    completionChart.AxisY = new AxesCollection();
                }

                completionChart.Series = SeriesCollection;

                completionChart.AxisX.Clear();
                completionChart.AxisX.Add(AxisX);

                completionChart.AxisY.Clear();
                completionChart.AxisY.Add(AxisY);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении диаграммы: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void OnApplyFiltersClick(object sender, RoutedEventArgs e)
        {
            var filteredPrograms = Programs;

            if (DepartmentFilter.SelectedItem is string department)
            {
                filteredPrograms = filteredPrograms.Where(p => p.Department == department).ToList();
            }

            if (PositionFilter.SelectedItem is string position)
            {
                filteredPrograms = filteredPrograms.Where(p => p.Position == position).ToList();
            }

            ProgramsDataGrid.ItemsSource = filteredPrograms;
            UpdateChart();
        }

        private void OpenEditWindow(AdaptationProgram program)
        {
            if (program == null) return;

            var editWindow = new EditProgramWindow(program);
            if (editWindow.ShowDialog() == true)
            {
                LoadData();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
