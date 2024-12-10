using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using WpfHR.Helpers;
using WpfHR.Models;



namespace WpfHR.ViewModels
{
    public class ModuleViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Module> Modules { get; set; }
        public ObservableCollection<string> Positions { get; set; }
        public ICollectionView ModulesView { get; }
        public ICommand CreateModuleCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        private string _selectedPosition;
        public string SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                _selectedPosition = value;
                OnPropertyChanged(nameof(SelectedPosition));
                ModulesView.Refresh();
            }
        }

        private Module _selectedModule;
        public Module SelectedModule
        {
            get => _selectedModule;
            set
            {
                if (_selectedModule != value)
                {
                    _selectedModule = value;
                    OnPropertyChanged(nameof(SelectedModule));
                }
            }
        }


        private ModuleDbContext _dbContext;
        public ModuleViewModel()
        {
            _dbContext = new ModuleDbContext();

            LoadModulesFromDatabase();

            Modules = new ObservableCollection<Module>(_dbContext.Modules);
            Positions = new ObservableCollection<string> { "Нет фильтра" };

            UpdatePositions();

            ModulesView = CollectionViewSource.GetDefaultView(Modules);
            ModulesView.Filter = FilterModules;

            DeleteCommand = new RelayCommand(DeleteSelectedModule, CanDeleteModule);

            ModulesView.Refresh();
        }
        private void LoadModulesFromDatabase()
        {
            var modules = _dbContext.Modules.ToList();
            Modules = new ObservableCollection<Module>(modules);
        }

        public void SaveModule()
        {
            _dbContext.SaveChanges();
        }

        public void SaveModule(Module module)
        {
            if (module != null)
            {
                var existingModule = _dbContext.Modules.FirstOrDefault(m => m.Id == module.Id);
                if (existingModule != null)
                {
                    if (!existingModule.Equals(module))
                    {
                        _dbContext.Entry(existingModule).CurrentValues.SetValues(module);
                    }
                }
                else
                {
                    _dbContext.Modules.Add(module);
                    Modules.Add(module);
                }
            }
            _dbContext.SaveChanges();
            UpdatePositions();
        }




        public void DeleteModule(Module module)
        {
            _dbContext.Modules.Remove(module);
            _dbContext.SaveChanges();
            Modules.Remove(module);
            UpdatePositions();
        }

        private void DeleteSelectedModule()
        {
            if (SelectedModule != null)
            {
                DeleteModule(SelectedModule);
            }
        }

        private bool CanDeleteModule()
        {
            return SelectedModule != null;
        }

        private bool FilterModules(object obj)
        {
            if (obj is not Module module) return false;

            return string.IsNullOrEmpty(SelectedPosition) ||
                   SelectedPosition == "Нет фильтра" ||
                   module.Position == SelectedPosition;
        }

        public string CustomMessage { get; set; }

        public void AddModule(Module module)
        {
            Modules.Add(module);

            if (!Positions.Contains(module.Position))
            {
                Positions.Add(module.Position);
                OnPropertyChanged(nameof(Positions));
            }

            ModulesView.Refresh();
            UpdatePositions();
        }

        private void UpdatePositions()
        {
            Positions.Clear();
            Positions.Add("Нет фильтра");
            foreach (var position in Modules.Select(m => m.Position).Distinct())
            {
                if (!string.IsNullOrEmpty(position))
                {
                    Positions.Add(position);
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
