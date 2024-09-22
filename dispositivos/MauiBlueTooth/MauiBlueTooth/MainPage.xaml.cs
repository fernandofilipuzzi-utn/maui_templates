using MauiBluetooth.Helpers;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System.Diagnostics;
using System.Text;

namespace MauiBluetooth
{
    public partial class MainPage : ContentPage
    {
        private IAdapter _adapter;
        private IBluetoothLE _ble;
        private IDevice selectedDevice;
        private List<IDevice> _gattDevices = new List<IDevice>();
        private List<IService> _services;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void btnScan_Clicked(object sender, EventArgs e)
        {
            var status = new CustomPermissionsHelper().RequestAllPermissionsAsync().Result;
            if (status != PermissionStatus.Granted)
            {
                return;
            }

            //si esta encendido 
            _ble = CrossBluetoothLE.Current;
            _ble.StateChanged += (s, e) =>
            {
                Debug.WriteLine($"The bluetooth state changed to {e.NewState}");
                lbStatus.Text = _ble.IsOn == false ? "bluetooth on" : "bluetooth off";
            };

            _adapter = CrossBluetoothLE.Current.Adapter;

            _adapter.ScanMode = ScanMode.Balanced;
            _adapter.ScanMatchMode = ScanMatchMode.AGRESSIVE;

            _gattDevices = new List<IDevice>();

            _adapter.DeviceDiscovered += (s, a) =>
            {
                _gattDevices.Add(a.Device);
                Debug.WriteLine($"Dispositivo encontrado: {a.Device.Name} - {a.Device.Id}");

                lbStatus.Text += $"Dispositivo encontrado: {a.Device.Name} - {a.Device.Id}";

                listView.ItemsSource = _gattDevices.ToArray();

                //    Dispatcher.Dispatch(() =>
                //    {
                //        listView.ItemsSource = null;
                //        listView.ItemsSource = _gattDevices.ToArray();
                //    });
            };

            if (_ble.IsAvailable == false || _ble.IsOn == false)
            {
                await DisplayAlert("Error", "Bluetooth is not available or enabled.", "OK");
                return;
            }

            lbStatus.Text += $"Escaneando...";
            await _adapter.StartScanningForDevicesAsync();
            lbStatus.Text += $"Find de escaneado...";

            var devices = _adapter.GetSystemConnectedOrPairedDevices().ToList();

            listView.ItemsSource = devices;
        }

        private async void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedDevice = e.Item as IDevice;
            if (selectedDevice != null)
            {
                _adapter?.ConnectToDeviceAsync(selectedDevice);
                await DisplayAlert("Connection done!", $"Connected to {selectedDevice.Name}", "Ok");


                var services = await selectedDevice.GetServicesAsync();

                _services = new List<IService>();
                foreach (var service in services)
                {
                    _services.Add(service );
                }
                listView1.ItemsSource = _services.ToArray();
            }
        }

        private async void listView1_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedService = e.Item as IService;
            if (selectedService != null)
            {

                var _characteristics = await selectedService.GetCharacteristicsAsync();
                 
                listView2.ItemsSource = _characteristics.ToArray();
            }
        }

        private async void listView2_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedCharacteric = e.Item as ICharacteristic;
            if (selectedCharacteric != null)
            {

                await selectedCharacteric.WriteAsync(Encoding.UTF8.GetBytes("1"));

                
            }
        }
    }
}