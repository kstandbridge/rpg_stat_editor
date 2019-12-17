using System.Windows;
using StatEditor.ViewModels;
using StatParser;

namespace StatEditor.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // TODO Unity/PRISM for dependency injection

            var entityFileLoader = new PersistenceManager();
            var statManager = new StatManager();
            DataContext = new MainViewModel(entityFileLoader, statManager);
        }
    }
}
