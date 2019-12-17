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
        private GameEntityDataViewModel _selectedData;
        private string _name;
        private string _type;
        private string _using;

        public GameEntityViewModel(string name)
        : this(new GameEntity{Name = name})
        {
        }

        public GameEntityViewModel(GameEntity gameEntity)
        {
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
                _using = value;
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