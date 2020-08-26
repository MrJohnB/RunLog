using System;
using System.ComponentModel.DataAnnotations;

namespace LiteBulb.RunLog.Dtos
{
	/// <summary>
	/// DTO to represent the Position object (AKA ViewModel).
	/// </summary>
	public class PositionDto
	{
		/// <summary>
		/// Id of the Position (generated internally by the database).
		/// </summary>
		[Display(Name = "Position Id")]
		//[Required]
		[Range(1, int.MaxValue)]
		public int? Id { get; set; }

		/// <summary>
		/// Id of Activity parent object this Position object is associated with.
		/// </summary>
		[Display(Name = "Activity Id")]
		[Required]
		[Range(1, int.MaxValue)]
		public int ActivityId { get; set; }

		/// <summary>
		/// DOMTimeStamp representing the time at which the location was retrieved.
		/// (DOMTimeStamp type represents an absolute or relative number of milliseconds, depending on the specification in which it appears.)
		/// </summary>
		[Display(Name = "Time Stamp")]
		[Required]
		[DataType(DataType.DateTime)]
		//[Range(typeof(DateTime), "1/1/1900", "12/31/2050", ErrorMessage = "Value for {0} must be between {1} and {2}")]
		public DateTime TimeStamp { get; set; }

		/// <summary>
		/// Position's latitude in decimal degrees.
		/// </summary>
		[Required]
		[Range(-90, 90)]
		public decimal Latitude { get; set; }

		/// <summary>
		/// Position's longitude in decimal degrees.
		/// </summary>
		[Required]
		[Range(-180, 80)]
		public decimal Longitude { get; set; }

		/// <summary>
		/// Position's altitude in meters, relative to sea level (this value can be null if the implementation cannot provide the data).
		/// </summary>
		[Range(-1419, 29029)]
		public decimal? Altitude { get; set; }

		/// <summary>
		/// Accuracy of the latitude and longitude properties (in meters).
		/// </summary>
		[Required]
		[Range(0, int.MaxValue)]
		public decimal Accuracy { get; set; }

		/// <summary>
		/// Accuracy of the altitude expressed in meters (this value can be null).
		/// </summary>
		[Display(Name = "Altitude Accuracy")]
		[Range(0, int.MaxValue)]
		public decimal? AltitudeAccuracy { get; set; }

		/// <summary>
		/// Direction in which the device is traveling. This value, specified in degrees, indicates how far off from heading true north the device is.
		/// 0 degrees represents true north, and the direction is determined clockwise (which means that east is 90 degrees and west is 270 degrees).
		/// If speed is 0, heading is NaN. (If the device is unable to provide heading information, this value is null).
		/// </summary>
		[Range(0, 360)]
		public decimal? Heading { get; set; }

		/// <summary>
		/// Velocity of the device in meters per second (this value can be null).
		/// </summary>
		[Range(0, int.MaxValue)]
		public decimal? Speed { get; set; }

		/// <summary>
		/// Number of satellites in view (this valud can be null).
		/// Note: might only work in native Android).
		/// </summary>
		[Display(Name = "Satellite Count")]
		[Range(0, int.MaxValue)]
		public int? SatelliteCount { get; set; }

		public PositionDto()
		{
			Id = null;
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
