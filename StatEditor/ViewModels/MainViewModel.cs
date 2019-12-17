using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using StatEditor.Properties;
using StatParser;

namespace StatEditor.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly PersistenceManager _persistenceManager;
        private readonly StatManager _statManager;
        private GameEntityViewModel _selectedItem;

        public MainViewModel(PersistenceManager persistenceManager, StatManager statManager)
        {
            _statManager = statManager;
            _persistenceManager = persistenceManager;
            LoadCommand = new CommandAsync(OnLoadCommand);
            SaveCommand = new CommandAsync(OnSaveCommand);
            GameEntities = new ObservableCollection<GameEntityViewModel>();
            EntityNames = new ObservableCollection<string>();
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
            await _persistenceManager.SaveEntitiesAsync("Data.txt", output).ConfigureAwait(false);
        }

        private async Task OnLoadCommand()
        {
            var input = await _persistenceManager.LoadEntitiesDataAsync("Data.txt");
            var entities = _statManager.Deserialize(input);
            GameEntities.Clear();
            foreach (var gameEntity in entities)
            {
                GameEntities.Add(new GameEntityViewModel(gameEntity));
            }

            EntityNames.Clear();
            EntityNames.Add(string.Empty);
            foreach (var name in entities.Select(e => e.Name).Distinct())
            {
                EntityNames.Add(name);
            }
        }

        public CommandAsync LoadCommand { get; }
        public CommandAsync SaveCommand { get; }

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
