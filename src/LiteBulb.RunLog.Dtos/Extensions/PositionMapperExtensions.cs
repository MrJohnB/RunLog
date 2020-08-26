using LiteBulb.RunLog.Models;
using System.Collections.Generic;

namespace LiteBulb.RunLog.Dtos.Extensions
{
	/// <summary>
	/// Extension methods that Position models to Position DTOs and vice versa.
	/// </summary>
	public static class PositionMapperExtensions
	{
		/// <summary>
		/// Maps a Position model object to a Position DTO object.
		/// </summary>
		/// <param name="position">Position object instance</param>
		/// <returns>Position DTO object instance</returns>
		public static PositionDto Map(this Position position)
		{
			return new PositionDto()
			{
				Id = position.Id,
				ActivityId = position.ActivityId,
				TimeStamp = position.TimeStamp,
				Latitude = position.Latitude,
				Longitude = position.Longitude,
				Altitude = position.Altitude,
				Accuracy = position.Accuracy,
				AltitudeAccuracy = position.AltitudeAccuracy,
				Heading = position.Heading,
				Speed = position.Speed,
				SatelliteCount = position.SatelliteCount
			};
		}

		/// <summary>
		/// Maps collection of Position model objects to Position DTO objects.
		/// </summary>
		/// <param name="positions">Collection of Position object instances</param>
		/// <returns>Collection of Position DTO objects</returns>
		public static IList<PositionDto> MapMany(this IEnumerable<Position> positions)
		{
			var positionsDtos = new List<PositionDto>();

			foreach (var position in positions)
				positionsDtos.Add(position.Map());

			return positionsDtos;
		}

		/// <summary>
		/// Maps a Position DTO object to a Position model object.
		/// </summary>
		/// <param name="positionDto">Position ob DTO Object instance</param>
		/// <returns>Position object instance</returns>
		public static Position Map(this PositionDto positionDto)
		{
			return new Position()
			{
				Id = positionDto.Id.HasValue ? positionDto.Id.Value : 0,
				ActivityId = positionDto.ActivityId,
				TimeStamp = positionDto.TimeStamp,
				Latitude = positionDto.Latitude,
				Longitude = positionDto.Longitude,
				Altitude = positionDto.Altitude,
				Accuracy = positionDto.Accuracy,
				AltitudeAccuracy = positionDto.AltitudeAccuracy,
				Heading = positionDto.Heading,
				Speed = positionDto.Speed,
				SatelliteCount = positionDto.SatelliteCount
			};
		}

		/// <summary>
		/// Maps collection of Position DTO objects to Position objects.
		/// </summary>
		/// <param name="positionsDtos">Collection of Position DTO object instances</param>
		/// <returns>Collection of Position objects</returns>
		public static IList<Position> MapMany(this IEnumerable<PositionDto> positionsDtos)
		{
			var positions = new List<Position>();

			foreach (var positionDto in positionsDtos)
				positions.Add(positionDto.Map());

			return positions;
		}
	}
}
