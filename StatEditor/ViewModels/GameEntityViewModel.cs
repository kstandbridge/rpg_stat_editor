using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using StatEditor.Properties;
using StatParser;

namespace StatEditor.ViewModels
{
    public class GameEntityViewModel : INotifyPropertyChanged
    {
        private readonly GameEntity _gameEntity;
        private GameEntityDataViewModel _selectedData;

        public GameEntityViewModel(GameEntity gameEntity)
        {
            _gameEntity = gameEntity;
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
            get => _gameEntity.Name;
            set
            {
                if (value == _gameEntity.Name) return;
                _gameEntity.Name = value;
                OnPropertyChanged();
            }
        }

        public string Type
        {
            get => _gameEntity.Type;
            set
            {
                if (value == _gameEntity.Type) return;
                _gameEntity.Type = value;
                OnPropertyChanged();
            }
        }

        public string Using
        {
            get => _gameEntity.Using;
            set
            {
                if (value == _gameEntity.Using) return;
                _gameEntity.Using = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<GameEntityDataViewModel> Data { get;  }

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