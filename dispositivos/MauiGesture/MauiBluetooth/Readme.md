

dotnet add package Plugin.BLE --prerelease -s=https://api.nuget.org/v3/index.json

https://www.nuget.org/packages/Plugin.BLE





https://www.c-sharpcorner.com/article/implementation-of-the-bluetooth-connectivity-using-net-maui/
https://medium.com/@chriszenzel/learn-a-new-job-skill-connecting-to-bluetooth-le-with-microsoft-net-maui-and-xamarin-ba3cf70f5d23
https://github.com/dotnet-bluetooth-le/dotnet-bluetooth-le


 ```
dotnet add package BarcodeScanner.Mobile.Maui --prerelease -s=https://api.nuget.org/v3/index.json
dotnet add package Microsoft.Maui.Controls.Compatibility --prerelease -s=https://api.nuget.org/v3/index.json
```

```
# dependencias duplicadas

<PackageReference Include="SomePackage">
    <ExcludeAssets>all</ExcludeAssets>
</PackageReference>

<application android:debuggable="true">
		<meta-data android:name="mono.log.level" android:value="debug" />
		<meta-data android:name="mono.debug" android:value="true" />
	</application>


<PropertyGroup Condition="'$(Configuration)|$(TargetFramework)|$(Platform)'=='Debug|net8.0-android34.0|AnyCPU'">
  <AndroidUseAapt2>True</AndroidUseAapt2>
  <AndroidCreatePackagePerAbi>False</AndroidCreatePackagePerAbi>
  <AndroidPackageFormat>apk</AndroidPackageFormat>
</PropertyGroup>
```

```
git init 
git remote add origin https://github.com/fernandofilipuzzi-utn/MauiCode.git
git pull origin main --rebase
git config user.name fernandofilipuzzi-utn
git config user.email fernandofilipuzzi.utn@gmail.com
git add *
git commit -m "."
git push --set-upstream origin main
```

https://www.nuget.org/packages/BarcodeScanning.Native.Maui/0.9.1
https://github.com/afriscic/BarcodeScanning.Native.Maui/tree/master/BarcodeScanning.Native.Maui
https://github.com/afriscic/BarcodeScanning.Native.Maui/blob/master/BarcodeScanning.Test/ScanTab.xaml.cs


siguiendo a este
https://github.com/dotnet-bluetooth-le/dotnet-bluetooth-le


videos
https://www.youtube.com/watch?v=ePbQC28hbiA


fundamentos
https://www.youtube.com/watch?v=fMvTwl3U1eo




este video me da error de java
https://www.youtube.com/watch?v=WWP2t-B5ADU
https://github.com/xamarin/GooglePlayServicesComponents/issues/872


https://learn.microsoft.com/es-es/dotnet/maui/tutorials/notes-app/?view=net-maui-8.0

https://github.com/dotnet-bluetooth-le/dotnet-bluetooth-le




```
//        private async void btnScanButton_Clicked(object sender, EventArgs e)
//        {
//            Dispatcher.Dispatch(async () =>
//            {
//                btnScanButton.IsEnabled = false;
//                _gattDevices.Clear();

//                lbStatus.Text = "Escaneando dispositivos...";
//                //var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));

//                try
//                {
//                    // await _adapter.StartScanningForDevicesAsync();// cancellationTokenSource.Token);

//                    if (_adapter.IsScanning == false)
//                    {
//                        await _adapter.StartScanningForDevicesAsync();
//                    }
//                    else
//                    {
//                        await DisplayAlert("Error", $"scanning pendiente", "OK");
//                    }

//                    //  await Task.Delay(TimeSpan.FromSeconds(20));
//                    // await _adapter.StopScanningForDevicesAsync();

//                    //adb logcat
//                     //await _adapter.StartScanningForDevicesAsync(new ScanFilterOptions
//                     //{
//                     //       ServiceUuids = new List<Guid> { // Uuids de servicios específicos 
//                     //}
//                     //});
             
//                }
//                catch (Exception ex)
//                {
//                    await DisplayAlert("Error", $"No se pudo iniciar el escaneo: {ex.Message}", "OK");
//                }
//                lbStatus.Text = "Escaneo completado.";

//                listView.ItemsSource = _gattDevices.ToArray();
//                btnScanButton.IsEnabled = true;
//            });

//        }

//        private async void btnConectar_Clicked(object sender, EventArgs e)
//        {
//#if ANDROID
//            try
//            {
//                Guid guid = Guid.Parse("00002A24-0000-1000-8000-00805F9B34FB");
//                var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
//                await _adapter.ConnectToKnownDeviceAsync(deviceGuid: guid, cancellationToken: cancellationTokenSource.Token);
//            }
//            catch (DeviceConnectionException ex)
//            {
//                await DisplayAlert("Error", $"No se puedo conectar: {ex.Message}", "OK");
//            }
//#endif
//        }
```