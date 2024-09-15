
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System.Text;

namespace MauiBlueTooth
{
    public partial class MainPage : ContentPage
    {
        IAdapter adapter = null;
        IService service = null;
        ICharacteristic characteristicUpdate = null;
        ICharacteristic characteristicWrite = null;

        string deviceId =               "00000000-0000-0000-0000-546c0e594305";
        string serviceId =              "0000fff0-0000-1000-8000-00805f9b34fb";
        string characteristicUpdateId = "0000fff4-0000-1000-8000-00805f9b34fb";
        string characteristicWriteId =  "0000fff1-0000-1000-8000-00805f9b34fb";


        public MainPage()
        {
            InitializeComponent();

            RequestPermissions();

            adapter = CrossBluetoothLE.Current.Adapter;

            adapter.DeviceConnected += async (s, e) =>
            {
                MainThread.BeginInvokeOnMainThread(() => SetStatusText("Connect."));
                MainThread.BeginInvokeOnMainThread(() => SetButtonText(btnConnectDisconnect, "Disconnect"));
            };

            adapter.DeviceDisconnected += async (s, e) =>
            {
                MainThread.BeginInvokeOnMainThread(() => SetStatusText("Disconnect."));
                MainThread.BeginInvokeOnMainThread(() => SetButtonText(btnConnectDisconnect, "Connect"));
            };

            var ble = CrossBluetoothLE.Current;

            var state = ble.State;

            ble.StateChanged += (s, e) => { };
        }

        private void SetStatusText(string status) 
        {
            lblError.Text = status;
        }

        private void SetButtonText(Button btn, string text)
        { 
            btn.Text = text;
        }

        private async void RequestPermissions()
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            { 
                status=await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }
        }

        private async void ConnectDevice()
        {
            try
            {
                var ble = CrossBluetoothLE.Current;

                if (ble.State == BluetoothState.On)
                {
                    var adapter = CrossBluetoothLE.Current.Adapter;
                    adapter.ScanTimeout = 5000;

                    var permissionStatus = await Permissions.CheckStatusAsync<BluetoothPermission>();

                    if (permissionStatus == PermissionStatus.Granted)
                    {
                        lblError.Text = "Searching for device...";
                        CancellationToken cancel = new CancellationToken();

                        adapter.ConnectToKnownDeviceAsync(Guid.Parse(deviceId), new ConnectParameters(true), cancel);
                    }
                    else
                    {
                        await Permissions.RequestAsync<BluetoothPermission>();
                    }
                }
                else
                {
                    lblError.Text = "Bluetooth is not on";
                }
            }
            catch (Exception ex)
            { 
                lblError.Text = ex.Message;
            }
        }

        private async void DisconnectDevice()
        {
            try
            {
                if (BtnStartStop.Text.Equals("Stop"))
                {
                    StopUpdates();
                }
                 
                lblError.Text = "Attempting to disconnect...";

                adapter.DisconnectDeviceAsync(adapter.ConnectedDevices[0]);
                
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private async Task<bool> FindService()
        {
            bool ok = false;
            try
            {
                MainThread.BeginInvokeOnMainThread(() => SetStatusText("Finding correct service..."));

                CancellationToken cancel = new CancellationToken();

                service = await adapter.ConnectedDevices[0].GetServiceAsync(Guid.Parse(serviceId), cancel);

                if (service != null)
                {
                    MainThread.BeginInvokeOnMainThread(() => SetStatusText("Service found."));
                    ok = true;
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(() => SetStatusText("Service not found."));
                    ok = true;
                }
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(() => SetStatusText(ex.Message));
            }

            return ok;
        }

        private async Task<bool> FindCharacteristics()
        {
            bool ok = false;
            try
            {
                MainThread.BeginInvokeOnMainThread(() => SetStatusText("Finding characteristics..."));

                Task<ICharacteristic>[] tasks = new Task<ICharacteristic>[] 
                                                { 
                                                   service.GetCharacteristicAsync( Guid.Parse(characteristicUpdateId)), 
                                                   service.GetCharacteristicAsync( Guid.Parse(characteristicWriteId)) 
                                                };

                var completeTasks = await Task.WhenAll(tasks);

                characteristicUpdate = completeTasks[0];
                characteristicWrite = completeTasks[1];

                if (characteristicUpdate != null && characteristicWrite != null)
                {
                    MainThread.BeginInvokeOnMainThread(() => SetStatusText("Characteristics found."));
                    MainThread.BeginInvokeOnMainThread(() => SetStatusText("Preparing event handler..."));

                    characteristicUpdate.ValueUpdated += (s, e) =>
                    {
                        try
                        {
                            S_ValueUpdate(s, e);
                        }
                        catch (Exception ex)
                        {
                            MainThread.BeginInvokeOnMainThread(() => SetStatusText(ex.Message));
                        }
                    };

                    ok = true;
                }
                else 
                {
                    MainThread.BeginInvokeOnMainThread(() => SetStatusText("Characterics not found."));
                    ok = true;
                }
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(() => SetStatusText(ex.Message));
            }

            return ok;
        }

        private void StartUpdates()
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() => SetButtonText(BtnStartStop,"Stop"));
                MainThread.BeginInvokeOnMainThread(() => SetStatusText("Listenning...."));

                characteristicWrite.WriteAsync(ASCIIEncoding.ASCII.GetBytes("Start Updates"));

                characteristicWrite.StartUpdatesAsync();
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(() => SetStatusText(ex.Message));
            }
        }

        private async void StopUpdates()
        {
            try
            {
                lblError.Text = "Stopping updates....";

                await characteristicWrite.WriteAsync(ASCIIEncoding.ASCII.GetBytes("Stop Update"));

                try
                {
                    await characteristicUpdate.StartUpdatesAsync();
                }
                catch (Exception ex)
                {
                    if (ex.Message.StartsWith("GATT: Write"))
                    { 
                    }
                }

                service = null;
                characteristicUpdate = null;
                characteristicWrite = null;

                lblError.Text = "Updates stopped";

                BtnStartStop.Text = "Start";
            }
            catch (Exception ex) 
            {
                lblError.Text = ex.Message;
            }
        }

        private void btnConnectDisconnect_Clicked(object sender, EventArgs e)
        {
            if (btnConnectDisconnect.Text=="Connect")
            {
                ConnectDevice();
            }
            else
            { 
                DisconnectDevice();
            }
        }

        private async void BtnStartStop_Clicked(object sender, EventArgs e)
        {
            if (BtnStartStop.Text=="Start")
            {
                if (await FindService())
                {
                    if (await FindCharacteristics())
                    {
                        StartUpdates();
                    }
                }
            }
            else 
            {
                StopUpdates();
            }
        }

        private void S_ValueUpdate(object sender, CharacteristicUpdatedEventArgs e)
        {
            try
            {
                var bytes = e.Characteristic.Value;
                var receivedData = Encoding.ASCII.GetString(bytes);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    lblError.Text = $"Data received: {receivedData}";
                });
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(() => SetStatusText(ex.Message));
            }
        }
    }

}
