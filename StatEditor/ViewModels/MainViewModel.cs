using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Win32;
using StatEditor.Properties;
using StatParser;

namespace StatEditor.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IPersistenceManager _persistenceManager;
        private readonly IStatManager _statManager;
        private GameEntityViewModel _selectedItem;

        public MainViewModel(
            IPersistenceManager persistenceManager,
            IStatManager statManager)
        {
            _statManager = statManager;
            _persistenceManager = persistenceManager;
            LoadCommand = new CommandAsync(OnLoadCommand);
            SaveCommand = new CommandAsync(OnSaveCommand, () => GameEntities.Any());
            GameEntities = new ObservableCollection<GameEntityViewModel>();
            EntityNames = new ObservableCollection<string>();
            AddCommand = new CommandAsync(OnAddCommand);
            RemoveCommand = new CommandAsync(OnRemoveCommand, () => SelectedItem != null);
        }

        private Task OnRemoveCommand()
        {
            GameEntities.Remove(SelectedItem);
            SaveCommand.NotifyCanExecuteChanged();

            return Task.CompletedTask;
        }

        private Task OnAddCommand()
        {
            GameEntities.Add(new GameEntityViewModel($"Entity_{GameEntities.Count + 1}", GameEntities));
            SaveCommand.NotifyCanExecuteChanged();
            return Task.CompletedTask;
        }

        private async Task OnSaveCommand()
        {
            var gameEntities = new List<GameEntity>();
            foreach (var vm in GameEntities)
            {
                var gameEntity = new GameEntity
                {
                    Name = vm.Name, Using = vm.Using, Type = vm.Type, Data = new Dictionary<string, string>()
                };
                foreach (var dataVm in vm.Data)
                {
                    gameEntity.Data.Add(dataVm.Type, dataVm.Value);
                }
                gameEntities.Add(gameEntity);
            }

            var output = _statManager.Serialize(gameEntities);
            var dialog = new SaveFileDialog()
            {
                InitialDirectory = Environment.CurrentDirectory, 
                FileName = "Data.txt"
            };
            dialog.ShowDialog();
            await _persistenceManager.SaveEntitiesAsync(dialog.FileName, output).ConfigureAwait(false);
        }

        private async Task OnLoadCommand()
        {
            var dialog = new OpenFileDialog
            {
                InitialDirectory = Environment.CurrentDirectory, 
                FileName = "Data.txt"
            };
            dialog.ShowDialog();
            var input = await _persistenceManager.LoadEntitiesDataAsync(dialog.FileName);
            var entities = _statManager.Deserialize(input);
            GameEntities.Clear();
            foreach (var gameEntity in entities)
            {
                GameEntities.Add(new GameEntityViewModel(gameEntity, GameEntities));
            }

            EntityNames.Clear();
            EntityNames.Add(string.Empty);
            foreach (var name in entities.Select(e => e.Name).Distinct())
            {
                EntityNames.Add(name);
            }
            SaveCommand.NotifyCanExecuteChanged();
        }

        public CommandAsync LoadCommand { get; }
        public CommandAsync SaveCommand { get; }
        public CommandAsync AddCommand { get; }
        public CommandAsync RemoveCommand { get; }

        public ObservableCollection<GameEntityViewModel> GameEntities { get; }

        public ObservableCollection<string> EntityNames { get; }

        public GameEntityViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value == _selectedItem) return;
                _selectedItem = value;
                OnPropertyChanged();
                RemoveCommand.NotifyCanExecuteChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
