using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace GeolocatorService
{
	/// <inheritdoc cref="IGeolocatorService"/>
	public class MockedLocationService : IGeolocatorService
	{
		private bool _isPermissionRequested;
		private bool _isPermissionGrantedWhenRequested;
		private Geocoordinate _geoCoordinate;

		public MockedLocationService(bool initialIsPermissionGrantedWhenRequested, Geocoordinate initialGeoCoordinate)
		{
			_isPermissionGrantedWhenRequested = initialIsPermissionGrantedWhenRequested;
			_geoCoordinate = initialGeoCoordinate;
		}

		public event LocationChangedEventHandler LocationChanged;
		public event LocationPermissionChangedEventHandler LocationPermissionChanged;

		public async Task<bool> GetIsPermissionGranted(CancellationToken ct)
		{
			return _isPermissionRequested && _isPermissionGrantedWhenRequested;
		}

		public async Task<Geocoordinate> GetLocation(CancellationToken ct)
		{
			if (await GetIsPermissionGranted(ct))
			{
				LocationChanged?.Invoke(this, new LocationChangedEventArgs(_geoCoordinate));

				return _geoCoordinate;
			}

			throw new InvalidOperationException("Location permission was not granted.");
		}

		public async Task<bool> RequestPermission(CancellationToken ct)
		{
			if (!_isPermissionRequested)
			{
				_isPermissionRequested = true;

				if (_isPermissionGrantedWhenRequested)
				{
					LocationPermissionChanged?.Invoke(this, new LocationPermissionChangedEventArgs(_isPermissionGrantedWhenRequested));
				}
			}

			return _isPermissionGrantedWhenRequested;
		}

		public void SetLocation(Geocoordinate coordinate)
		{
			_geoCoordinate = coordinate;
			LocationChanged?.Invoke(this, new LocationChangedEventArgs(_geoCoordinate));
		}

		public void SetIsPermissionGrantedWhenRequested(bool isPermissionGranted)
		{
			_isPermissionGrantedWhenRequested = isPermissionGranted;
			if (_isPermissionRequested)
			{
				LocationPermissionChanged?.Invoke(this, new LocationPermissionChangedEventArgs(_isPermissionGrantedWhenRequested));
			}
		}
	}
}
