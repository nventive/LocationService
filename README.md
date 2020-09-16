# GeolocatorService

A service to get the user's current location, based on Geolocator, suited to UWP or [Uno](https://platform.uno/) apps.

GeolocatorService aims to simplify getting the user's current location and handling most common scenarios, including getting the permission to obtain this location and handling cases where this permission is denied.

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)

## Getting Started

Add the GeolocatorService nuget package to your project heads.

To have access to reactive extensions such as ```GetAndObserveLocation```, add the GeolocatorService.Reactive nuget package to your project.

### Add the relevant permissions

#### UWP

Add the Location capability in your manifest.

#### iOS

Add the NSLocationWhenInUsageDescription and NSLocationUsageDescription values to your info.plist file. This is the message that is displayed to your user when the permission for their location is requested. For instance,

```
<key>NSLocationWhenInUseUsageDescription</key>
<string>TODO NSLocationWhenInUsageDescription</string>
<key>NSLocationUsageDescription</key>
<string>TODO NSLocationUsageDescription</string>
```

#### Android

Add the location permission to your AssemblyInfo.cs file.

```
[assembly: UsesPermission(Android.Manifest.Permission.AccessFineLocation)]
```

### Request permission

After you have added the relevant location permission info to your project configuration, locate the spot in your code where you want to ask the user for their location permission. This [guide](https://developer.apple.com/design/human-interface-guidelines/ios/app-architecture/requesting-permission/) or this [guide](https://developer.android.com/training/permissions/requesting) may help you decide. This must be done before accessing their location. Add the following line to do so:

```
var isGranted = await locationService.RequestPermission(ct);
```

You may safely request the permission multiple times; the user will only get prompted as often as is necessary.

For instance, if you ask the user for their permission once and they decline, calling `RequestPermission` again will not prompt the user again, but simply return `false`.

If the user answers "Allow once", then calling `RequestPermission` again during the lifetime of the app will simply return `true`. Once the app is restarted, calling `RequestPermission` will prompt them for the permission once again, since their original permission has now expired.

Once permission is requested, you can use the various methods and extension methods of IGeolocatorService.

## Features

### Request the user's location

Once you have obtained the location permission, it's a simple matter to obtain the location:

```
var location = await locationService.GetLocation(ct);
```

If the user has not granted the location permission, an `InvalidOperationException` is thrown; therefore, we recommend not calling this method if you know the user has denied the location permission and this is a normal scenario. If the app has not kept track of this information, we recommend calling this extension method instead:

```
var locationOrDefault = await locationService.GetLocationOrDefault(ct);
```

This method will return null instead of throwing an exception, in case the permission was denied.

### React to changes in the user's location or their permissions

`IGeolocatorService` offers events to allow you to react when the user's location has changed, or when they have granted or denied permissions. Even if these changes occur while the app is in background, they will be raised when the app comes back to the foreground.

```
locationService.LocationChanged += OnLocationChanged;
locationService.LocationPermissionChanged += OnLocationPermissionChanged;
```

### Track the location and permission using Reactive Extensions

GeolocatorService.Reactive adds a few extension methods which will allow your app to fluidly keep track of the user's location and permission status.

For instance, let's say your app wants to display relevant information to the user's location when it's available, and a completely different type of information when that location is unavailable (because the permission was denied or the device is unable to provide a location).

```
IObservable<PageContent> content = locationService.GetAndObserveLocationOrDefault(ct)
    .Select(location =>
    {
        if (location == null)
        {
            // TODO Gets information which is independent of the location
        }
        else
        {
            // TODO Gets information pertinent to the location
        }
    });
```

## Changelog

Please consult the [CHANGELOG](CHANGELOG.md) for more information about version
history.

## License

This project is licensed under the Apache 2.0 license - see the
[LICENSE](LICENSE) file for details.

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on the process for
contributing to this project.

Be mindful of our [Code of Conduct](CODE_OF_CONDUCT.md).

## Contributors

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tr>
    <td align="center"><a href="https://github.com/jcantin-nventive"><img src="https://avatars3.githubusercontent.com/u/43351943?v=4" width="100px;" alt=""/><br /><sub><b>Julie Cantin</b></sub></a><br /><a href="https://github.com/nventive/Chinook.BackButtonManager/commits?author=jcantin-nventive" title="Code">💻 📖</a></td>
  </tr>
</table>

<!-- markdownlint-enable -->
<!-- prettier-ignore-end -->
<!-- ALL-CONTRIBUTORS-LIST:END -->

This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification. Contributions of any kind welcome!
