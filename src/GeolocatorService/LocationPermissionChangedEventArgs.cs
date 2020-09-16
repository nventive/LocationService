using System;
using System.Collections.Generic;
using System.Text;

namespace GeolocatorService
{
	public class LocationPermissionChangedEventArgs : EventArgs
	{
		public LocationPermissionChangedEventArgs(bool isLocationPermissionGranted)
		{
			IsLocationPermissionGranted = isLocationPermissionGranted;
		}

		public bool IsLocationPermissionGranted { get; }
	}
}
