## Notas









1- no he actualizado ningún nuget

2- dependencias.

```bash
dotnet add package BarcodeScanner.Mobile.Maui --prerelease -s=https://api.nuget.org/v3/index.json
dotnet add MauiQR.csproj package Microsoft.Maui.Controls.Compatibility --prerelease -s=https://api.nuget.org/v3/index.json
```

3- compilar por comando

```bash
dotnet clean
dotnet restore
dotnet build -f net8.0-android
dotnet build -f net8.0-android34.0
dotnet publish -f:net8.0-android -c:Release -o ./publish --self-containeddon
```

## Configuración del proyecto

Destildar
elegir
- Destino para la plataforma Android: Antroid 14.0 (nivel de API34)
- Destilar : Implementación rápida  : Debug@net8.0-android34

no es necesario configurar el target para api34


actualizar

```
Microsoft.Maui.Controls
Microsoft.Maui.Controls.Compatibility

dotnet new maui -n MauiQR

dotnet clean
dotnet restore
dotnet build
dotnet build -f net8.0-android
dotnet publish -f:net8.0-android -c:Release -o ./publish --self-contained

dotnet add MauiQR.csproj package BarcodeScanner.Mobile.Maui --prerelease -s=https://api.nuget.org/v3/index.json
dotnet add MauiQR.csproj package Microsoft.Maui.Controls.Compatibility --prerelease -s=https://api.nuget.org/v3/index.json
```

dotnet build -f net8.0-android34.0
dotnet publish -f:net8.0-android34.0 -c:Release -o ./publish --self-contained
git pull origin main --rebase

dotnet workload list
dotnet workload update

# dependencias duplicadas
<PackageReference Include="SomePackage">
    <ExcludeAssets>all</ExcludeAssets>
</PackageReference>

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


## Este video me da error de java

https://www.youtube.com/watch?v=WWP2t-B5ADU
https://github.com/xamarin/GooglePlayServicesComponents/issues/872



pasos

### instalar nuget

```
BarcodeScanner.Mobile.Maui 8.0.40.1
```

### agregar el handler

MauiProgram.cs
```csharp
using BarcodeScanner.Mobile;

...

builder
    ...
    .ConfigureMauiHandlers(handlers =>
    {
        handlers.AddBarcodeScannerHandler();
    });
```

### modificar el 

```
	<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
	<uses-permission android:name="android.permission.INTERNET" />
	<uses-permission android:name="android.permission.VIBRATE" />
	<uses-permission android:name="android.permission.FLASHLIGHT" />
	<uses-permission android:name="android.permission.CAMERA" />
```

### target: motorola moto g42

Configuración

```
aplicación/destinos Android/destinos para la plataforma android /Android 14.0 (nivel de api 34)
   cambie el manifest - no
  
```

Desactivar la compilación para windows no 
```   
<TargetFrameworks Condition="$([MSBuild]::IsOSPlatform('windows'))">$(TargetFrameworks);net8.0-windows10.0.19041.0</TargetFrameworks>
```

cambiando esto 
```
   <PackageReference Include="Microsoft.Maui.Controls.Compatibility" Version="$(MauiVersion)" />
```
   por
```
   <PackageReference Include="Microsoft.Maui.Controls.Compatibility" Version="9.0.0-rc.1.24453.9" />
```  
   
### desactivando implementación rapida
```
android/opciones/implementación rápida - false
```

### Target
```
android/opciones/release & net8-android34.0     tilde pak
```


 
### Modificar el proyecto con lo siguiente.
 ```
 	<ItemGroup>
	...
		<PackageReference Include="Xamarin.AndroidX.Activity" Version="1.9.0.2" Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'android'" />
		<PackageReference Include="Xamarin.AndroidX.Activity.Ktx" Version="1.9.0.2" Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'android'" />
		<PackageReference Include="BarcodeScanner.Mobile.Maui" Version="8.0.40.1" />
	</ItemGroup>
 ```

 ```
Gravedad	Código	Descripción	Proyecto	Archivo	Línea	Estado suprimido
Error (activo)	XAGR7019	System.UnauthorizedAccessException: Acceso denegado a la ruta de acceso 'C:\Users\fernando\source\repos\MauiApp9\MauiApp9\obj\Release\net8.0-android34.0\lp\209\jl\res\values-bg\values.xml'.
   en System.IO.__Error.WinIOError(Int32 errorCode, String maybeFullPath)
   en System.IO.FileStream.Init(String path, FileMode mode, FileAccess access, Int32 rights, Boolean useRights, FileShare share, Int32 bufferSize, FileOptions options, SECURITY_ATTRIBUTES secAttrs, String msgPath, Boolean bFromProxy, Boolean useLongPath, Boolean checkHost)
   en System.IO.FileStream..ctor(String path, FileMode mode, FileAccess access, FileShare share, Int32 bufferSize)
   en System.Xml.XmlDownloadManager.GetStream(Uri uri, ICredentials credentials, IWebProxy proxy, RequestCachePolicy cachePolicy)
   en System.Xml.XmlUrlResolver.GetEntity(Uri absoluteUri, String role, Type ofObjectToReturn)
   en System.Xml.XmlTextReaderImpl.FinishInitUriString()
   en System.Xml.XmlReaderSettings.CreateReader(String inputUri, XmlParserContext inputContext)
   en Xamarin.Android.Tasks.FileResourceParser.ProcessXmlFile(String file, Dictionary`2 resources)
   en Xamarin.Android.Tasks.FileResourceParser.ProcessResourceFile(String file, Dictionary`2 resources)
   en Xamarin.Android.Tasks.FileResourceParser.Parse(String resourceDirectory, IEnumerable`1 additionalResourceDirectories, Dictionary`2 resourceMap)
   en Xamarin.Android.Tasks.GenerateRtxt.RunTask()
   en Microsoft.Android.Build.Tasks.AndroidTask.Execute() en /Users/runner/work/1/s/xamarin-android/external/xamarin-android-tools/src/Microsoft.Android.Build.BaseTasks/AndroidTask.cs:línea 25	MauiApp9 (net8.0-android34.0)	C:\Program Files\dotnet\packs\Microsoft.Android.Sdk.Windows\34.0.138\tools\Xamarin.Android.Resource.Designer.targets	65
   ```