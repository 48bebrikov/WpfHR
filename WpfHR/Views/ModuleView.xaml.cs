using System.Windows.Controls;
using System.Windows;
using WpfHR.ViewModels;
using System;
using WpfHR.Services;
using Microsoft.Win32;
using System.Windows.Input;
using WpfHR.Models;


namespace WpfHR.Views
{
    public partial class ModuleView : UserControl
    {
        private readonly PdfService _pdfService;

        public ModuleView()
        {
            InitializeComponent();
            this.DataContext = new ModuleViewModel();

            _pdfService = new PdfService();

            this.Unloaded += OnModuleViewUnloaded;
        }

        private void OnSendNotificationClick(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as ModuleViewModel;

            if (viewModel?.SelectedModule != null)
            {
                try
                {
                    var saveFileDialog = new SaveFileDialog
                    {
                        Title = "Сохранить PDF файл",
                        Filter = "PDF файлы (*.pdf)|*.pdf",
                        FileName = $"{viewModel.SelectedModule.CodeName}_Order.pdf"
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string filePath = saveFileDialog.FileName;

                        // Генерация PDF через PdfService
                        var pdfService = new PdfService();
                        pdfService.GeneratePdfOrder(viewModel.SelectedModule, filePath);

                        MessageBox.Show($"PDF успешно сохранён по пути: {filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при создании PDF: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите модуль для создания PDF.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OnModuleViewUnloaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ModuleViewModel;
            viewModel?.SaveModule();
        }

        private void OnCreateModuleClick(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as ModuleViewModel;
            if (viewModel == null) return;

            var createModuleWindow = new CreateModuleWindow(viewModel);
            if (createModuleWindow.ShowDialog() == true)
            {
                var newModule = createModuleWindow.NewModule;
                if (newModule != null)
                {
                    viewModel.AddModule(newModule);
                }
            }
        }
        private void ModulesDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid != null)
            {
                if (dataGrid.SelectedItem is Module editedModule)
                {
                    var viewModel = this.DataContext as ModuleViewModel;
                    viewModel?.SaveModule(editedModule);
                }
                dataGrid.CommitEdit();
                dataGrid.IsReadOnly = true;
            }
        }


        private void ModulesDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.CurrentCell != null)
            {
                if (dataGrid.SelectedItem != null && dataGrid.CurrentCell.IsValid)
                {
                    dataGrid.IsReadOnly = false;
                    dataGrid.BeginEdit();
                }
            }
        }


        private void ModulesDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.F2)
            {
                e.Handled = true;
            }
        }


        private void ModulesDataGrid_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            var dataGrid = sender as DataGrid;
        }
    }
}
