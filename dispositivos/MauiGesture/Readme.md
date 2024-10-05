
conexión realizada

https://stackoverflow.com/questions/78764712/net-maui-ble-app-not-showing-any-services


## Depedencias
```
dotnet add package Plugin.BLE --prerelease -s=https://api.nuget.org/v3/index.json
dotnet add package System.Xml.XmlSerializer
dotnet add package System.Linq.Expressions
```

## Documentación
[Plugin.BLE](https://www.nuget.org/packages/Plugin.BLE)


## Construir con comandos
```
dotnet new maui -n MauiQR

dotnet clean
dotnet restore
dotnet build
dotnet build -f net8.0-android
dotnet publish -f:net8.0-android -c:Release -o ./publish --self-contained
```

```
dotnet build -f net8.0-android34.0
dotnet publish -f:net8.0-android34.0 -c:Release -o ./publish --self-contained
```

```
git pull origin main --rebase
```

```
dotnet workload list
dotnet workload update
```

ver
Microsoft.Maui.Graphics


MvvmCross.Plugin.BLE


Plugin.BluetoothLE

Shiny.BluetoothLE
Shiny.Hosting.Maui
https://shinylib.net/client/blehosting/gatt/

Plugin.BluetoothLE

https://github.com/aritchie/bluetoothle/blob/master/Samples/Samples/ScanViewModel.cs
