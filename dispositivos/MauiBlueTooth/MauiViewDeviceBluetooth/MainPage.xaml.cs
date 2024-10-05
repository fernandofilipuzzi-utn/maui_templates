using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE;
using System.Collections.ObjectModel;
using MauiViewDeviceBluetooth.Helpers;
using System.Text;
using Plugin.BLE.Abstractions;
using System.Windows.Input;
using Microsoft.Maui.Devices;
using System.Globalization;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiViewDeviceBluetooth
{
    public partial class MainPage : ContentPage
    {
        private IAdapter _bluetoothAdapter;
        private ObservableCollection<BluetoothDeviceViewModel> DiscoveredDevices { get; set; } = new ObservableCollection<BluetoothDeviceViewModel>();

        public MainPage()
        {
            InitializeComponent();

            InitializeBluetooth();
        }

        private void InitializeBluetooth()
        {
            _bluetoothAdapter = CrossBluetoothLE.Current.Adapter;
          //  DiscoveredDevices = new ObservableCollection<BluetoothDeviceViewModel>();
              deviceListView.ItemsSource = DiscoveredDevices;

            _bluetoothAdapter.DeviceDiscovered += BluetoothAdapter_DeviceDiscovered;
        }

        private async void OnScanButtonClicked(object sender, EventArgs e)
        {
            DiscoveredDevices.Clear();

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

        private async void BluetoothAdapter_DeviceDiscovered(object sender, DeviceEventArgs deviceEventArgs)
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

            # region descubre los dispositivos
            if (!DiscoveredDevices.Any(d => d.Address == macAddress))
            {
                var newDevice = new BluetoothDeviceViewModel
                {
                    Name = deviceInfo.Name ?? $"Dispositivo Desconocido",
                    IdString = $"{deviceEventArgs.Device.Id}",
                    Rssi = deviceInfo.Rssi.ToString(),
                    Address = macAddress,
                    DeviceInfo= deviceInfo
                };
                                
                DiscoveredDevices.Add(newDevice);
            }
            #endregion
        }

        private async Task SendValueToCharacteristicAsync(BluetoothCharacteristicViewModel characteristic, string value)
        {
            if (characteristic.CanWrite)
            {
                var valueToSend = "1";
                await characteristic.Characteristic.WriteAsync(Encoding.UTF8.GetBytes(valueToSend));
            }
        }
    }

    public class BluetoothDeviceViewModel : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string IdString { get; set; } 
        public string Rssi { get; set; } 
        public string Address { get; set; }
        bool _isConnected;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                if (_isConnected != value)
                {
                    _isConnected = value;
                     OnPropertyChanged(nameof(IsConnected)); // Notificación de cambio
                }
            }
        }
        public IDevice DeviceInfo { get; set; }
        
        public ObservableCollection<BluetoothServiceViewModel> Services { get; set; } = new ObservableCollection<BluetoothServiceViewModel>(); // Lista de servicios

        public ICommand ConnectDisconnectCommand => new Command<BluetoothDeviceViewModel>(async (device) =>
        {
            if (device.IsConnected)
            {
                await DisconnectDeviceAsync(device);
            }
            else
            {
                await ConnectDeviceAsync(device);
            }
        });

        private async Task ConnectDeviceAsync(BluetoothDeviceViewModel device)
        {
            try
            {
                var deviceInfo = device.DeviceInfo;
                await CrossBluetoothLE.Current.Adapter.ConnectToDeviceAsync(deviceInfo);
                device.IsConnected = true;

                #region descubre los servicios y caracteristicas
                var services = await deviceInfo.GetServicesAsync();
                foreach (var service in services)
                {
                    var newService = new BluetoothServiceViewModel
                    {
                        Name = service.Name,
                        Id = service.Id.ToString()
                    };

                    var characteristics = await service.GetCharacteristicsAsync();
                    foreach (var characteristic in characteristics)
                    {
                        var characteristicViewModel = new BluetoothCharacteristicViewModel(characteristic);
                        newService.Characteristics.Add(characteristicViewModel);
                    }

                    device.Services.Add(newService);
                }
                #endregion
            }
            catch (Exception ex)
            {
               // await DisplayAlert("Error", $"No se pudo conectar al dispositivo: {ex.Message}", "OK");
            }
        }

        private async Task DisconnectDeviceAsync(BluetoothDeviceViewModel device)
        {
            device.IsConnected = false;
        }
    }

    public class BluetoothDevice
    {
        public string Name { get; set; }
        public string IdString { get; set; }
        public int Rssi { get; set; }
        public string Address { get; set; }
    }

    public class BluetoothServiceViewModel
    {
        string name;
        public string Name { get { return $"Service: {name}"; } set { name = value; } } 
        public string Id { get; set; }
        public ObservableCollection<BluetoothCharacteristicViewModel> Characteristics { get; set; } = new ObservableCollection<BluetoothCharacteristicViewModel>(); 
    }

    public class BluetoothCharacteristicViewModel
    {
        private string _name;
        public string Name
        {
            get => $"Characteristic: {_name}"; 
            set => _name = value;
        }
        public string Id { get; set; }
        public string Uuid { get; set; }

        public ICharacteristic Characteristic { get; set; } 

        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanNotify { get; set; } 

        public BluetoothCharacteristicViewModel(ICharacteristic characteristic)
        {
            Characteristic = characteristic;
            Id = characteristic.Id.ToString();
            Uuid = characteristic.Uuid.ToString();
            Name = characteristic.Name ?? "Desconocida"; 

            // Inicializa las capacidades de la característica
            CanRead = characteristic.Properties.HasFlag(CharacteristicPropertyType.Read);
            CanWrite = characteristic.Properties.HasFlag(CharacteristicPropertyType.Write) || characteristic.Properties.HasFlag(CharacteristicPropertyType.WriteWithoutResponse);
            CanNotify = characteristic.Properties.HasFlag(CharacteristicPropertyType.Notify);
        }

        public ICommand SendValueCommand => new Command<BluetoothCharacteristicViewModel>(async (characteristic) =>
        {
            if (characteristic.CanWrite)
            {
                //var valueToSend = "1"; 
                //await characteristic.Characteristic.WriteAsync(Encoding.UTF8.GetBytes(valueToSend));
                await characteristic.Characteristic.WriteAsync(Encoding.UTF8.GetBytes("1"));
            }
        });
    }

    public class BoolToConnectDisconnectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Desconectar" : "Conectar";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;//throw new NotImplementedException();
        }
    }
}
