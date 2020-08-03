using System;

namespace LiteBulb.RunLog.Web.ViewModels
{
	public class PositionViewModel
	{
		/// <summary>
		/// Id of Activity parent object this Position object is associated with.
		/// </summary>
		public int ActivityId { get; set; }

		/// <summary>
		/// DOMTimeStamp representing the time at which the location was retrieved.
		/// (DOMTimeStamp type represents an absolute or relative number of milliseconds, depending on the specification in which it appears.)
		///TODO: conversion??
		/// </summary>
		public DateTime TimeStamp { get; set; }

		/// <summary>
		/// Position's latitude in decimal degrees
		/// </summary>
		public double Latitude { get; set; }

		/// <summary>
		/// Position's longitude in decimal degrees.
		/// </summary>
		public double Longitude { get; set; }

		/// <summary>
		/// Position's altitude in meters, relative to sea level (this value can be null if the implementation cannot provide the data).
		/// </summary>
		public double? Altitude { get; set; }

		/// <summary>
		/// Accuracy of the latitude and longitude properties (in meters).
		/// </summary>
		public double Accuracy { get; set; }

		/// <summary>
		/// Accuracy of the altitude expressed in meters (this value can be null).
		/// </summary>
		public double? AltitudeAccuracy { get; set; }

		/// <summary>
		/// Direction in which the device is traveling. This value, specified in degrees, indicates how far off from heading true north the device is.
		/// 0 degrees represents true north, and the direction is determined clockwise (which means that east is 90 degrees and west is 270 degrees).
		/// If speed is 0, heading is NaN. (If the device is unable to provide heading information, this value is null).
		/// </summary>
		public double? Heading { get; set; }

		/// <summary>
		/// Velocity of the device in meters per second (this value can be null).
		/// </summary>
		public double? Speed { get; set; }

		/// <summary>
		/// Number of satellites in view (note: might only work in native Android).
		/// </summary>
		public int? SatelliteCount { get; set; }

		public PositionViewModel()
		{
			ActivityId = int.MinValue;
			TimeStamp = DateTime.MinValue;
			Latitude = double.MinValue;
			Longitude = double.MinValue;
			Altitude = null;
			Accuracy = double.MinValue;
			AltitudeAccuracy = null;
			Heading = null;
			Speed = null;
			SatelliteCount = null;
		}

		public static implicit operator Models.Position(PositionViewModel positionViewModel)
		{
			return new Models.Position()
			{
				ActivityId = positionViewModel.ActivityId,
				TimeStamp = positionViewModel.TimeStamp,
				Latitude = (decimal)positionViewModel.Latitude,
				Longitude = (decimal)positionViewModel.Longitude,
				Altitude = (decimal?)positionViewModel.Altitude,
				Accuracy = (decimal)positionViewModel.Accuracy,
				AltitudeAccuracy = (decimal?)positionViewModel.AltitudeAccuracy,
				Heading = (decimal?)positionViewModel.Heading,
				Speed = (decimal?)positionViewModel.Speed,
				SatelliteCount = positionViewModel.SatelliteCount
			};
		}
	}
}
