using System.ComponentModel;
using System.Runtime.CompilerServices;
using StatEditor.Properties;

namespace StatEditor.ViewModels
{
    public class GameEntityDataViewModel : INotifyPropertyChanged
    {
        private string _value;
        private string _type;

        public GameEntityDataViewModel(string type, string value)
        {
            Type = type;
            Value = value;
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

        public string Value
        {
            get => _value;
            set
            {
                if (value == _value) return;
                _value = value;
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