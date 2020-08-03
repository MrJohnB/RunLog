using LiteBulb.Common.DataModel;
using System;

namespace LiteBulb.RunLog.Models
{
	/// <summary>
	/// POCO to represent the Position object.
	/// </summary>
	public class Position : BaseModel<int>
	{
		/// <summary>
		/// Id of Activity parent object this Position object is associated with.
		/// </summary>
		public int ActivityId { get; set; }

		/// <summary>
		/// DOMTimeStamp representing the time at which the location was retrieved.
		/// (DOMTimeStamp type represents an absolute or relative number of milliseconds, depending on the specification in which it appears.)
		/// </summary>
		public DateTime TimeStamp { get; set; }

		/// <summary>
		/// Position's latitude in decimal degrees.
		/// </summary>
		public decimal Latitude { get; set; }

		/// <summary>
		/// Position's longitude in decimal degrees.
		/// </summary>
		public decimal Longitude { get; set; }

		/// <summary>
		/// Position's altitude in meters, relative to sea level (this value can be null if the implementation cannot provide the data).
		/// </summary>
		public decimal? Altitude { get; set; }

		/// <summary>
		/// Accuracy of the latitude and longitude properties (in meters).
		/// </summary>
		public decimal Accuracy { get; set; }

		/// <summary>
		/// Accuracy of the altitude expressed in meters (this value can be null).
		/// </summary>
		public decimal? AltitudeAccuracy { get; set; }

		/// <summary>
		/// Direction in which the device is traveling. This value, specified in degrees, indicates how far off from heading true north the device is.
		/// 0 degrees represents true north, and the direction is determined clockwise (which means that east is 90 degrees and west is 270 degrees).
		/// If speed is 0, heading is NaN. (If the device is unable to provide heading information, this value is null).
		/// </summary>
		public decimal? Heading { get; set; }

		/// <summary>
		/// Velocity of the device in meters per second (this value can be null).
		/// </summary>
		public decimal? Speed { get; set; }

		/// <summary>
		/// Number of satellites in view (this valud can be null).
		/// Note: might only work in native Android).
		/// </summary>
		public int? SatelliteCount { get; set; }

		public Position()
		{
			Id = int.MinValue;
			ActivityId = int.MinValue;
			TimeStamp = DateTime.MinValue;
			Latitude = decimal.MinValue;
			Longitude = decimal.MinValue;
			Altitude = null;
			Accuracy = decimal.MinValue;
			AltitudeAccuracy = null;
			Heading = null;
			Speed = null;
			SatelliteCount = null;
		}
	}
}
