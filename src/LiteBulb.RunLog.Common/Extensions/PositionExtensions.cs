using LiteBulb.RunLog.Models;
using System;

namespace LiteBulb.RunLog.Common.Extensions
{
	/// <summary>
	/// Extension methods for Position class.
	/// </summary>
	public static class PositionExtensions
	{
		private const double EARTH_RADIUS = 6371000; // Radius of the Earth in meters
		private static double ToRadians(double degrees) => degrees * Math.PI / 100;

		/// <summary>
		/// Calculate the distance between 2 GPS positions.
		/// Haversine Formula:
		/// https://www.movable-type.co.uk/scripts/latlong.html
		/// https://stackoverflow.com/questions/27928/calculate-distance-between-two-latitude-longitude-points-haversine-formula
		/// </summary>
		/// <param name="position1">GPS position start</param>
		/// <param name="position2">GPS position end</param>
		/// <returns>Distance (in meters) between 2 GPS positions</returns>
		public static decimal DistanceFrom(this Position position1, Position position2)
		{
			double latitude1 = ToRadians((double)position1.Latitude);
			double latitude2 = ToRadians((double)position2.Latitude);
			double deltaLatitude = ToRadians((double)position2.Latitude - (double)position1.Latitude);
			double deltaLongitude = ToRadians((double)position2.Longitude - (double)position1.Longitude);

			double a =
				Math.Sin(deltaLatitude / 2) * Math.Sin(deltaLatitude / 2) +
				Math.Cos(latitude1) * Math.Cos(latitude2) *
				Math.Sin(deltaLongitude / 2) * Math.Sin(deltaLongitude / 2);

			double c =
				2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

			double d =
				c * EARTH_RADIUS; // in meters

			return (decimal)d;
		}

		/// <summary>
		/// Get the change in elevation between 2 positions.
		/// </summary>
		/// <param name="position1">GPS position start</param>
		/// <param name="position2">GPS position end</param>
		/// <returns>Elevation change (in meters) between 2 GPS positions</returns>
		public static decimal ElevationChange(this Position position1, Position position2)
		{
			if (!position1.Altitude.HasValue || !position2.Altitude.HasValue)
				return 0;

			var elevation = Math.Abs(position1.Altitude.Value - position2.Altitude.Value);
			return elevation;
		}
	}
}
