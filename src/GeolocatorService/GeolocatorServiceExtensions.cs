using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace GeolocatorService
{
	public static class GeolocatorServiceExtensions
	{
		/// <summary>
		/// Gets the current location. Does not throw an <see cref="InvalidOperationException"/> when that location is unavailable
		/// (for instance, because of lacking permissions) but rather returns null.
		/// </summary>
		public static async Task<Geocoordinate> GetLocationOrDefault(this IGeolocatorService locationService, CancellationToken ct)
		{
			try
			{
				return await locationService.GetLocation(ct);
			}
			catch (InvalidOperationException)
			{
				return null;
			}
		}

		/// <summary>
		/// Attempts to obtain the location and indicates whether this was successful. See <see cref="LocationResult"/>
		/// </summary>
		public static async Task<LocationResult> TryGetLocation(this IGeolocatorService locationService, CancellationToken ct)
		{
			try
			{
				return new LocationResult (isSuccessful: true, location: await locationService.GetLocation(ct));
			}
			catch (InvalidOperationException)
			{
				return new LocationResult(isSuccessful: false, location: default);
			}
		}
	}
}
