using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE;
using System.Collections.ObjectModel;
using MauiViewDeviceBluetooth.Helpers;
using System.Text;
using Plugin.BLE.Abstractions;
using System.Windows.Input;

namespace MauiViewDeviceBluetooth
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

            // Verifica si el dispositivo ya ha sido descubierto
            if (!_discoveredDevices.Any(d => d.Address == macAddress))
            {
                var newDevice = new BluetoothDeviceViewModel
                {
                    Name = deviceInfo.Name ?? $"Dispositivo Desconocido",
                    IdString = $"{deviceEventArgs.Device.Id}",
                    Rssi = deviceInfo.Rssi.ToString(),
                    Address = macAddress
                };

                // Conectar al dispositivo
                try
                {
                    await _bluetoothAdapter.ConnectToDeviceAsync(deviceInfo);
                    // Ahora intenta obtener los servicios del dispositivo conectado
                    var services = await deviceInfo.GetServicesAsync();
                    foreach (var service in services)
                    {
                        var newService = new BluetoothServiceViewModel
                        {
                            Name = service.Name,
                            Id = service.Id.ToString()
                        };

                        // Obtener características de cada servicio
                        var characteristics = await service.GetCharacteristicsAsync();
                        foreach (var characteristic in characteristics)
                        {
                            var characteristicViewModel = new BluetoothCharacteristicViewModel(characteristic);
                            newService.Characteristics.Add(characteristicViewModel);
                        }

                        newDevice.Services.Add(newService);
                    }
                }
                catch (Exception ex)
                {
                    // Maneja cualquier excepción que pueda ocurrir al conectar o al obtener servicios
                    await DisplayAlert("Error", $"No se pudo conectar al dispositivo: {ex.Message}", "OK");
                }

                // Agregar el dispositivo a la lista después de intentar conectarse y obtener servicios
                _discoveredDevices.Add(newDevice);
            }
        }

        public ICommand SendValueCommand => new Command<BluetoothCharacteristicViewModel>(async (characteristic) =>
        {
            if (characteristic.CanWrite)
            {
                // Aquí puedes obtener el valor del Entry relacionado
                var valueToSend = "1"; // Reemplaza esto con el valor ingresado en el Entry

                // Escribe el valor en la característica
                await characteristic.Characteristic.WriteAsync(Encoding.UTF8.GetBytes(valueToSend));
            }
        });
        /*
        private async void CharacteristicListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedCharacteristic = e.Item as BluetoothCharacteristicViewModel;
            if (selectedCharacteristic != null)
            {
                // Obtener la característica real desde el dispositivo
                var service = (sender as ListView).BindingContext as BluetoothServiceViewModel;
                var device = (service.BindingContext as BluetoothDeviceViewModel);
                var characteristic = await device.Device.GetCharacteristicAsync(selectedCharacteristic.Id);

                if (characteristic != null)
                {
                    // Escribir datos en la característica
                    await characteristic.WriteAsync(Encoding.UTF8.GetBytes("1"));
                }
            }

         var selectedCharacteric = e.Item as ICharacteristic;
            if (selectedCharacteric != null)
            {

                await selectedCharacteric.WriteAsync(Encoding.UTF8.GetBytes("1"));

                
            }
        */
    }

    public class BluetoothDeviceViewModel
    {
        public string Name { get; set; }
        public string IdString { get; set; } 
        public string Rssi { get; set; } 
        public string Address { get; set; }
        public ObservableCollection<BluetoothServiceViewModel> Services { get; set; } = new ObservableCollection<BluetoothServiceViewModel>(); // Lista de servicios
    }

    public class BluetoothServiceViewModel
    {
        string name;
        public string Name { get { return $"Service: {name}"; } set { name = value; } } // Nombre del servicio
        public string Id { get; set; } // ID del servicio
        public ObservableCollection<BluetoothCharacteristicViewModel> Characteristics { get; set; } = new ObservableCollection<BluetoothCharacteristicViewModel>(); // Lista de características
    }

    public class BluetoothCharacteristicViewModel
    {
        private string _name;
        public string Name
        {
            get => $"Characteristic: {_name}"; // Formateo del nombre de la característica
            set => _name = value;
        }

        public string Id { get; set; } // ID de la característica

        public string Uuid { get; set; } // UUID de la característica

        public ICharacteristic Characteristic { get; set; } // Propiedad de referencia a la característica

        // Propiedades adicionales
        public bool CanRead { get; set; } // Indica si la característica se puede leer
        public bool CanWrite { get; set; } // Indica si la característica se puede escribir
        public bool CanNotify { get; set; } // Indica si la característica tiene notificaciones

        // Constructor para inicializar propiedades
        public BluetoothCharacteristicViewModel(ICharacteristic characteristic)
        {
            Characteristic = characteristic;
            Id = characteristic.Id.ToString();
            Uuid = characteristic.Uuid.ToString();
            Name = characteristic.Name ?? "Desconocida"; // Asignar un nombre predeterminado si es nulo

            // Inicializa las capacidades de la característica
            CanRead = characteristic.Properties.HasFlag(CharacteristicPropertyType.Read);
            CanWrite = characteristic.Properties.HasFlag(CharacteristicPropertyType.Write);
            CanNotify = characteristic.Properties.HasFlag(CharacteristicPropertyType.Notify);
        }
    }
}
