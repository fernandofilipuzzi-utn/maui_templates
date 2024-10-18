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

        private void Button_Clicked(object sender, EventArgs e)
        {
            (BindingContext as MainViewModel)?.RefreshCommand.Execute(this);
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

        public event PropertyChangedEventHandler? PropertyChanged;

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
            Items.Clear();
            Items.Add("Item A");
            Items.Add("Item B");
            Items.Add("Item C");
            Items.Add("Item D");

            IsRefreshing = false;
        }

        private async void ExecuteManualRefreshCommand()
        {
            
            await Task.Delay(10000);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Items.Clear();
                Items.Add("Manual Item 1");
                Items.Add("Manual Item 2");
                Items.Add("Manual Item 3");

                IsRefreshing = false;
            });
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
