using LiteBulb.Common.DataModel;
using LiteBulb.RunLog.Models.Enumerations;
using System;
using System.Collections.Generic;

//TODO: units of measurement (i.e. English or metric)?
namespace LiteBulb.RunLog.Models
{
	/// <summary>
	/// POCO to represent the Activity object.
	/// </summary>
	public class Activity : BaseModel<int>
	{
		/// <summary>
		/// Name of the athlete doing the Activity.
		/// </summary>
		public string RunnerName { get; set; }

		/// <summary>
		/// Type of Activity being performed (Walk, Run, Bike, Swim).
		/// </summary>
		public ActivityType Type { get; set; }

		/// <summary>
		/// Status of the Activity (Pending, Started, Paused, Complete).
		/// </summary>
		public ActivityStatus Status { get; set; }

		/// <summary>
		/// Time the Activity object was created in the system.
		/// </summary>
		public DateTime CreatedAt { get; set; }

		/// <summary>
		/// Start time for this Activity.
		/// </summary>
		public DateTime StartedAt { get; set; }

		/// <summary>
		/// End time for this Activity.
		/// </summary>
		public DateTime CompletedAt { get; set; }

		/// <summary>
		/// Description containing extra details about the Activity.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// System of measurement units used for this Activity (i.e. English or Metric).
		/// </summary>
		public DistanceUnits Units { get; set; }

		/// <summary>
		/// Collection of Position coordinates that belong to this Activity.
		/// </summary>
		public List<Position> Positions { get; set; }

		/// <summary>
		/// Number of Position objects that belong to this Activity.
		/// Note: this is an optimization property so we don't have to map Position children to each Activity,
		/// but we can still display the count.  This will be used when retrieving a list of Activities,
		/// but not when getting a single Activity.
		/// TODO: fix this later with ViewModel.
		/// </summary>
		public int? PositionCount { get; set; }

		public Activity()
		{
			Id = int.MinValue;
			RunnerName = string.Empty;
			Type = ActivityType.Undefined;
			Status = ActivityStatus.Undefined;
			CreatedAt = DateTime.MinValue;
			StartedAt = DateTime.MinValue;
			CompletedAt = DateTime.MinValue;
			Description = string.Empty;
			Units = DistanceUnits.Undefined;
			Positions = new List<Position>();
			PositionCount = null;
		}
	}
}
