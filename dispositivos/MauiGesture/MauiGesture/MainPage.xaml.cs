using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace MauiGesture
{
    public partial class MainPage : ContentPage
    {
       //

        public MainPage()
        {
            InitializeComponent();

            //RefreshView refreshView = new RefreshView();
            //ICommand refreshCommand = new Command(() =>
            //{
            //    // IsRefreshing is true
            //    // Refresh data here
            //    refreshView.IsRefreshing = false;
            //});
            //refreshView.Command = refreshCommand;

            //ScrollView scrollView = new ScrollView();
            //FlexLayout flexLayout = new FlexLayout { };
            //scrollView.Content = flexLayout;
            //refreshView.Content = scrollView;

            BindingContext = new MainViewModel();
        }

        
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                if (_isRefreshing != value)
                {
                    _isRefreshing = value;
                    OnPropertyChanged(nameof(IsRefreshing));
                }
            }
        }

        public ObservableCollection<string> Items { get; set; }

        public ICommand RefreshCommand { get; }
        public ICommand ManualRefreshCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            Items = new ObservableCollection<string>
            {
                "Item 1", "Item 2", "Item 3", "Item 4"
            };

            RefreshCommand = new Command(ExecuteRefreshCommand);
            ManualRefreshCommand = new Command(ExecuteManualRefreshCommand);
        }

        private void ExecuteRefreshCommand()
        {
            // Simular una recarga de datos
            Items.Clear();
            Items.Add("Item A");
            Items.Add("Item B");
            Items.Add("Item C");
            Items.Add("Item D");

            // Terminar el refresco
            IsRefreshing = false;
        }

        private void ExecuteManualRefreshCommand()
        {
            // Simular una recarga de datos diferente
            Items.Clear();
            Items.Add("Manual Item 1");
            Items.Add("Manual Item 2");
            Items.Add("Manual Item 3");

            // Detener el refresco
            IsRefreshing = false;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
