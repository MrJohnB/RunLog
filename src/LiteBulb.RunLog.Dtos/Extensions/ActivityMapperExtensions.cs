using LiteBulb.RunLog.Models;
using System.Collections.Generic;

namespace LiteBulb.RunLog.Dtos.Extensions
{
	/// <summary>
	/// Extension methods that map Activity models to Activity DTOs and vice versa.
	/// </summary>
	public static class ActivityMapperExtensions
	{
		/// <summary>
		/// Maps a Activity model object to a Activity DTO object.
		/// </summary>
		/// <param name="activity">Activity object instance</param>
		/// <returns>Activity DTO object instance</returns>
		public static ActivityDto Map(this Activity activity)
		{
			return new ActivityDto()
			{
				Id = activity.Id > 0 ? activity.Id : (int?)null,
				AthleteName = activity.AthleteName,
				Type = activity.Type,
				Status = activity.Status,
				CreatedAt = activity.CreatedAt,
				StartedAt = activity.StartedAt,
				CompletedAt = activity.CompletedAt,
				Description = activity.Description,
				Units = activity.Units,
				Positions = activity.Positions.MapMany(),
				PositionCount = activity.PositionCount
			};
		}

		/// <summary>
		/// Maps collection of Activity model objects to Activity DTO objects.
		///	Big O 2n? => return activities.Select(x => x.Map()).ToArray();
		/// </summary>
		/// <param name="activities">Collection of Activity object instances</param>
		/// <returns>Collection of Activity DTO objects</returns>
		public static IList<ActivityDto> MapMany(this IEnumerable<Activity> activities)
		{
			var activityDtos = new List<ActivityDto>();

			foreach (var activity in activities)
				activityDtos.Add(activity.Map());

			return activityDtos;
		}

		/// <summary>
		/// Maps a Activity DTO object to a Activity model object.
		/// </summary>
		/// <param name="activityDto">Activity DTO object instance</param>
		/// <returns>Activity object instance</returns>
		public static Activity Map(this ActivityDto activityDto)
		{
			return new Activity()
			{
				Id = activityDto.Id.HasValue ? activityDto.Id.Value : 0,
				AthleteName = activityDto.AthleteName,
				Type = activityDto.Type,
				Status = activityDto.Status,
				CreatedAt = activityDto.CreatedAt,
				StartedAt = activityDto.StartedAt,
				CompletedAt = activityDto.CompletedAt,
				Description = activityDto.Description,
				Units = activityDto.Units,
				Positions = activityDto.Positions.MapMany(),
				PositionCount = activityDto.PositionCount
			};
		}

		/// <summary>
		/// Maps collection o Activity DTO objects to Activity model objects.
		/// </summary>
		/// <param name="activityDtos">Collection of Activity DTO object instances</param>
		/// <returns>Collection of Activity objects</returns>
		public static IList<Activity> MapMany(this IEnumerable<ActivityDto> activityDtos)
		{
			var activities = new List<Activity>();

			foreach (var activityDto in activityDtos)
				activities.Add(activityDto.Map());

			return activities;
		}
	}
}
