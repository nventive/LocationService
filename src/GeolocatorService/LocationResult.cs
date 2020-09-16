using System;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Geolocation;

namespace GeolocatorService
{
	/// <summary>
	/// This represents the result of trying to obtain a location.
	/// </summary>
	public class LocationResult
	{
		public LocationResult(bool isSuccessful, Geocoordinate location)
		{
			IsSuccessful = isSuccessful;
			Location = location;
		}

		/// <summary>
		/// Indicates whether the attempt to retrieve the location was successful.
		/// </summary>
		public bool IsSuccessful { get; }

		/// <summary>
		/// The location which was retrieved. Null if <see cref="IsSuccessful"/> is false.
		/// </summary>
		public Geocoordinate Location { get; }
	}
}
