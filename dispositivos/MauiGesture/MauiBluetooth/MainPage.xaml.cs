using MauiBluetooth.Helpers;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System.Collections.ObjectModel;
namespace MauiBluetooth
{
    public partial class MainPage : ContentPage
    {
        private readonly IAdapter _bluetoothAdapter;
        private ObservableCollection<IDevice> _discoveredDevices;

        public MainPage()
        {
            InitializeComponent();

            _bluetoothAdapter = CrossBluetoothLE.Current.Adapter;
            _discoveredDevices = new ObservableCollection<IDevice>();
            deviceListView.ItemsSource = _discoveredDevices;
        }

        private async void OnScanButtonClicked(object sender, EventArgs e)
        {

            _discoveredDevices.Clear();

            var status = new CustomPermissionsHelper().RequestAllPermissionsAsync().Result;
            if (status != PermissionStatus.Granted)
            {
                return;
            }

            if (CrossBluetoothLE.Current.State == BluetoothState.Off)
            {
                await DisplayAlert("Error", "Bluetooth está apagado. Por favor enciéndelo.", "OK");
                return;
            }

            _bluetoothAdapter.DeviceDiscovered += (s, deviceEventArgs) =>
            {
               // if (deviceEventArgs.Device.Name != null) // Solo mostrar dispositivos con nombre
            //    {
                    _discoveredDevices.Add(deviceEventArgs.Device);
             //   }
            };

            lbEstado.Text = "escaneando";
            await _bluetoothAdapter.StartScanningForDevicesAsync();
            lbEstado.Text = "Fin Escaneado";
            await _bluetoothAdapter.StopScanningForDevicesAsync();
        }
    }
}