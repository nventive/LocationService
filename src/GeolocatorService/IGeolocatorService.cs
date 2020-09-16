using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace GeolocatorService
{
	/// <summary>
	/// A service which handles the permission to get the user's location and tracks that location.
	/// </summary>
	public interface IGeolocatorService
	{
		/// <summary>
		/// Gets the current location. If the permission is not already granted, throws an InvalidOperationException.
		/// </summary>
		Task<Geocoordinate> GetLocation(CancellationToken ct);

		/// <summary>
		/// Gets an event indicating that the user location has changed.
		/// </summary>
		event LocationChangedEventHandler LocationChanged;

		/// <summary>
		/// Requests the location permission from the user. May do nothing if the permission was already requested.
		/// </summary>
		Task<bool> RequestPermission(CancellationToken ct);

		/// <summary>
		/// Gets whether the user has granted the location permission, without prompting them.
		/// </summary>
		Task<bool> GetIsPermissionGranted(CancellationToken ct);

		/// <summary>
		/// Gets an event indicating that the permission status has changed.
		/// </summary>
		event LocationPermissionChangedEventHandler LocationPermissionChanged;
	}

	/// <summary>
	/// Occurs when the user location changes.
	/// </summary>
	public delegate void LocationChangedEventHandler(object sender, LocationChangedEventArgs property);

	/// <summary>
	/// Occurs when the location permission changes (granted or denied)
	/// </summary>
	public delegate void LocationPermissionChangedEventHandler(object sender, LocationPermissionChangedEventArgs property);
}
