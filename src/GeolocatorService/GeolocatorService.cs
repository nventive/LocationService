using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace GeolocatorService
{
	/// <inheritdoc cref="IGeolocatorService"/>
	public class GeolocatorService : IGeolocatorService
	{
		private readonly Geolocator _locator;

		public GeolocatorService()
		{
			_locator = new Geolocator();
		}

		public event LocationChangedEventHandler LocationChanged;
		public event LocationPermissionChangedEventHandler LocationPermissionChanged;

		public async Task<bool> GetIsPermissionGranted(CancellationToken ct)
		{
			return _locator.LocationStatus == PositionStatus.Ready;
		}

		public async Task<Geocoordinate> GetLocation(CancellationToken ct)
		{
			var position = await _locator.GetGeopositionAsync().AsTask(ct);
			return position.Coordinate;
		}

		public async Task<bool> RequestPermission(CancellationToken ct)
		{
			// Only subscribe to these events here, not in the ctor, because subscribing to these
			// Geolocator events causes the permission to be immediately requested and we want to allow
			// greater flexibility with the moment the permission is requested.
			_locator.StatusChanged -= OnStatusChanged;
			_locator.StatusChanged += OnStatusChanged;

			_locator.PositionChanged -= OnPositionChanged;
			_locator.PositionChanged += OnPositionChanged;

			var status = await Geolocator.RequestAccessAsync().AsTask(ct);

			return status == GeolocationAccessStatus.Allowed;
		}

		private void OnPositionChanged(Geolocator sender, PositionChangedEventArgs args)
		{
			LocationChanged?.Invoke(sender, new LocationChangedEventArgs(args.Position.Coordinate));
		}

		private void OnStatusChanged(Geolocator sender, StatusChangedEventArgs args)
		{
			LocationPermissionChanged?.Invoke(sender, new LocationPermissionChangedEventArgs(args.Status == PositionStatus.Ready));
		}
	}
}
