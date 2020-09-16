using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace GeolocatorService
{
	public static class GeocoordinateExtensions
	{
		/// <summary>
		/// Gets a short string indicating the longitude and latitude of the geocoordinate, or [null] if there is not a
		/// valid longitude and latitude
		/// </summary>
		/// <param name="geocoordinate"></param>
		/// <returns></returns>
		public static string ToShortString(this Geocoordinate geocoordinate)
		{
			if (geocoordinate == null
				|| geocoordinate.Point.Position.Latitude == double.NaN
				|| geocoordinate.Point.Position.Longitude == double.NaN)
			{
				return "[null]";
			}

			return $"({geocoordinate.Point.Position.Longitude}, {geocoordinate.Point.Position.Latitude})";
		}
	}
}
