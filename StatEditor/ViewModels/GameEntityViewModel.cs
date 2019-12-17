using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using StatEditor.Properties;
using StatParser;

namespace StatEditor.ViewModels
{
    public class GameEntityViewModel : INotifyPropertyChanged
    {
        private readonly GameEntity _gameEntity;
        private readonly ObservableCollection<GameEntityViewModel> _entities;
        private GameEntityDataViewModel _selectedData;
        private string _name;
        private string _type;
        private string _using;

        public GameEntityViewModel(string name, ObservableCollection<GameEntityViewModel> entities)
        : this(new GameEntity{Name = name}, entities)
        {
        }

        public GameEntityViewModel(GameEntity gameEntity, ObservableCollection<GameEntityViewModel> entities)
        {
            _gameEntity = gameEntity;
            _entities = entities;
            _name = gameEntity.Name;
            _type = gameEntity.Type;
            _using = gameEntity.Using;
            Data = new ObservableCollection<GameEntityDataViewModel>();
            foreach (var data in gameEntity.Data)
            {
                Data.Add(new GameEntityDataViewModel(data.Key, data.Value));
            }
            AddCommand = new CommandAsync(OnAddCommand);
            RemoveCommand = new CommandAsync(OnRemoveCommand, () => SelectedData != null);
        }

        private Task OnRemoveCommand()
        {
            Data.Remove(SelectedData);
            return Task.CompletedTask;
        }

        private Task OnAddCommand()
        {
            Data.Add(new GameEntityDataViewModel($"Type_{Data.Count + 1}", string.Empty));
            return Task.CompletedTask;
        }

        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Type
        {
            get => _type;
            set
            {
                if (value == _type) return;
                _type = value;
                OnPropertyChanged();
            }
        }

        public string Using
        {
            get => _using;
            set
            {
                if (value == _using) return;
                RemoveInheritedProperties(_entities.FirstOrDefault(e => e.Name == _using));
                _using = value;
                OnPropertyChanged();
                AddInheritedProperties(_entities.FirstOrDefault(e => e.Name == _using));
            }
        }

        private void RemoveInheritedProperties(GameEntityViewModel parent)
        {
            if (parent == null) return;
            foreach (var data in Data.ToList())
            {
                var existingData = parent.Data.FirstOrDefault(d => d.Type == data.Type);
                if (existingData != null && existingData.Value == data.Value)
                {
                    Data.Remove(data);
                }
            }
        }

        private void AddInheritedProperties(GameEntityViewModel parent)
        {
            if (parent == null) return;
            foreach (var data in parent.Data)
            {
                var existingData = Data.FirstOrDefault(d => d.Type == data.Type);
                if (existingData == null)
                {
                    Data.Add(new GameEntityDataViewModel(data.Type, data.Value));
                }
            }
        }

        public ObservableCollection<GameEntityDataViewModel> Data { get; set; }

        public GameEntityDataViewModel SelectedData
        {
            get => _selectedData;
            set
            {
                if (value == _selectedData) return;
                _selectedData = value;
                OnPropertyChanged();
                RemoveCommand.NotifyCanExecuteChanged();
            }
        }

        public CommandAsync AddCommand { get; }
        public CommandAsync RemoveCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}