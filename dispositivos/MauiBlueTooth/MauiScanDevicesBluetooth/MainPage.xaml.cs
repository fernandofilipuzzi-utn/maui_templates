using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE;
using System.Collections.ObjectModel;
using MauiScanDevicesBluetooth.Helpers;
using Plugin.BLE.Abstractions.Extensions;
using Plugin.BLE.Abstractions.EventArgs;

namespace MauiScanDevicesBluetooth
{
    public partial class MainPage : ContentPage
    {
        private readonly IAdapter _bluetoothAdapter;
        private ObservableCollection<BluetoothDeviceViewModel> _discoveredDevices;

        public MainPage()
        {
            InitializeComponent();

            _bluetoothAdapter = CrossBluetoothLE.Current.Adapter;
            _discoveredDevices = new ObservableCollection<BluetoothDeviceViewModel>();
            deviceListView.ItemsSource = _discoveredDevices;

            // Inicializar el evento una vez
            _bluetoothAdapter.DeviceDiscovered += BluetoothAdapter_DeviceDiscovered;
        }

        private async void OnScanButtonClicked(object sender, EventArgs e)
        {
            // Limpiar la lista de dispositivos descubiertos
            _discoveredDevices.Clear();

            var status = await new CustomPermissionsHelper().RequestAllPermissionsAsync();
            if (status != PermissionStatus.Granted)
            {
                return;
            }

            if (CrossBluetoothLE.Current.State == BluetoothState.Off)
            {
                await DisplayAlert("Error", "Bluetooth está apagado. Por favor enciéndelo.", "OK");
                return;
            }

            lbEstado.Text = "escaneando";
            await _bluetoothAdapter.StartScanningForDevicesAsync();
            lbEstado.Text = "Fin Escaneado";
            await _bluetoothAdapter.StopScanningForDevicesAsync();
        }

        private void BluetoothAdapter_DeviceDiscovered(object sender, DeviceEventArgs deviceEventArgs)
        {
            var deviceInfo = deviceEventArgs.Device;
            var macAddress = string.Empty;

            if (deviceInfo.NativeDevice != null)
            {
                var addressProperty = deviceInfo.NativeDevice.GetType().GetProperty("Address");
                if (addressProperty != null)
                {
                    macAddress = addressProperty.GetValue(deviceInfo.NativeDevice)?.ToString();
                }
            }

            // Verificar si el dispositivo ya está en la lista antes de agregar
            if (!_discoveredDevices.Any(d => d.Address == macAddress))
            {
                _discoveredDevices.Add(new BluetoothDeviceViewModel
                {
                    Name = deviceInfo.Name ?? $"Dispositivo Desconocido",
                    IdString = $"{deviceEventArgs.Device.Id}",
                    Rssi = deviceInfo.Rssi.ToString(),
                    Address = macAddress
                });
            }
        }

        /*
        private readonly IAdapter _bluetoothAdapter;
        //private ObservableCollection<IDevice> _discoveredDevices;
        private ObservableCollection<BluetoothDeviceViewModel> _discoveredDevices;
        
        public MainPage()
        {
            InitializeComponent();

            _bluetoothAdapter = CrossBluetoothLE.Current.Adapter;
            //_discoveredDevices = new ObservableCollection<IDevice>();
            _discoveredDevices = new ObservableCollection<BluetoothDeviceViewModel>();
            deviceListView.ItemsSource = _discoveredDevices;
        }

        private async void OnScanButtonClicked(object sender, EventArgs e)
        {
            // Desuscribirse de eventos previos para evitar duplicaciones
            _bluetoothAdapter.DeviceDiscovered -= BluetoothAdapter_DeviceDiscovered;

            // Suscribirse al evento de nuevo
            _bluetoothAdapter.DeviceDiscovered += BluetoothAdapter_DeviceDiscovered;

            // Limpiar la lista de dispositivos descubiertos
            _discoveredDevices.Clear();

            var status = await new CustomPermissionsHelper().RequestAllPermissionsAsync();
            if (status != PermissionStatus.Granted)
            {
                return;
            }

            if (CrossBluetoothLE.Current.State == BluetoothState.Off)
            {
                await DisplayAlert("Error", "Bluetooth está apagado. Por favor enciéndelo.", "OK");
                return;
            }

            lbEstado.Text = "escaneando";
            await _bluetoothAdapter.StartScanningForDevicesAsync();
            lbEstado.Text = "Fin Escaneado";
            await _bluetoothAdapter.StopScanningForDevicesAsync();
        }

        // Método separado para manejar el evento DeviceDiscovered
        private void BluetoothAdapter_DeviceDiscovered(object sender, DeviceEventArgs deviceEventArgs)
        {
            var deviceInfo = deviceEventArgs.Device;
            var macAddress = string.Empty;

            if (deviceInfo.NativeDevice != null)
            {
                var addressProperty = deviceInfo.NativeDevice.GetType().GetProperty("Address");
                if (addressProperty != null)
                {
                    macAddress = addressProperty.GetValue(deviceInfo.NativeDevice)?.ToString();
                }
            }

            // Verificar si el dispositivo ya está en la lista antes de agregar
            if (!_discoveredDevices.Any(d => d.Address == macAddress))
            {
                _discoveredDevices.Add(new BluetoothDeviceViewModel
                {
                    Name = deviceInfo.Name ?? $"Dispositivo Desconocido",
                    IdString = $"{deviceEventArgs.Device.Id}",
                    Rssi = deviceInfo.Rssi.ToString(),
                    Address = macAddress
                });
            }
        }
        */
    }

    public class BluetoothDeviceViewModel
    {
        public string Name { get; set; }
        public string IdString { get; set; } // Este es el Id convertido a string

        public string Rssi { get; set; } // Este es el Id convertido a string

        public string Address { get; set; } // Este es el Id convertido a string
    }

}
