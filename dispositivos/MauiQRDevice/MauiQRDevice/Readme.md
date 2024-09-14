nota, no actualizar solo correr 
dotnet add MauiQR.csproj package BarcodeScanner.Mobile.Maui --prerelease -s=https://api.nuget.org/v3/index.json
dotnet add MauiQR.csproj package Microsoft.Maui.Controls.Compatibility --prerelease -s=https://api.nuget.org/v3/index.json
dotnet clean
dotnet restore
dotnet build -f net8.0-android

no es necesario configurar el target para api34


actualizar
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


git init 
git remote add origin https://github.com/fernandofilipuzzi-utn/MauiCode.git
git pull origin main --rebase
git config user.name fernandofilipuzzi-utn
git config user.email fernandofilipuzzi.utn@gmail.com
git add *
git commit -m "."
git push --set-upstream origin main


https://www.nuget.org/packages/BarcodeScanning.Native.Maui/0.9.1
https://github.com/afriscic/BarcodeScanning.Native.Maui/tree/master/BarcodeScanning.Native.Maui
https://github.com/afriscic/BarcodeScanning.Native.Maui/blob/master/BarcodeScanning.Test/ScanTab.xaml.cs


este video me da error de java
https://www.youtube.com/watch?v=WWP2t-B5ADU
https://github.com/xamarin/GooglePlayServicesComponents/issues/872