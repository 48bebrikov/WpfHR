using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WpfHR.Models;
using WpfHR.ViewModels;


namespace WpfHR.Views
{
    public partial class CreateModuleWindow : Window
    {
        public Module NewModule { get; private set; }
        private ModuleViewModel ViewModel { get; }

        public CreateModuleWindow(ModuleViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;

            // Инициализация выпадающего списка главных согласующих
            MainApproverComboBox.ItemsSource = ViewModel.Modules
                .SelectMany(m => m.Approvers)
                .Distinct()
                .ToList();
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Если нужно сделать обязательные поля
                if (//string.IsNullOrWhiteSpace(CodeNameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(DevelopersTextBox.Text) ||
                    string.IsNullOrWhiteSpace(ApproversTextBox.Text)
                    //string.IsNullOrWhiteSpace(PositionTextBox.Text
                    )
                {
                    // Просто показываем предупреждение и создаем черновик
                    var draftModule = new Module
                    {
                        CodeName = CodeNameTextBox.Text.Trim(),
                        Name = "Черновик",
                        Developers = new List<string>(),
                        Approvers = new List<string>(),
                        MainApprover = "",
                        Position = PositionTextBox.Text.Trim(),
                        Deadline = DateTime.Now.AddDays(0),
                        Status = "Черновик",
                        CustomMessage = $"Добрый день, коллеги. Вы назначены разработчиком адаптационного модуля \"{CodeNameTextBox.Text.Trim()}\". Просим ознакомиться с приказом и приступить к работе."
                    };

                    ViewModel.SaveModule(draftModule);

                    MessageBox.Show("Вы не указали необходимые поля. Создается Черновик!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var developers = DevelopersTextBox.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                        .Select(d => d.Trim())
                                                        .ToList();

                var module = new Module
                {
                    CodeName = CodeNameTextBox.Text.Trim(),
                    Name = NameTextBox.Text.Trim(),
                    Developers = developers,
                    Approvers = ApproversTextBox.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()).ToList(),
                    MainApprover = MainApproverComboBox.Text.Trim(),
                    Position = PositionTextBox.Text.Trim(),
                    Deadline = DeadlineDatePicker.SelectedDate ?? DateTime.Now,
                    CustomMessage = MessageTextBox.Text.Trim(),
                    Status = "Создан"
                };

                ViewModel.SaveModule(module);

                MessageBox.Show("Модуль успешно сохранён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
 