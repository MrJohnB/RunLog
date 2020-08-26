using LiteBulb.RunLog.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LiteBulb.RunLog.Dtos
{
	/// <summary>
	/// DTO to represent the Activity object (AKA ViewModel).
	/// </summary>
	public class ActivityDto
	{
		/// <summary>
		/// Id of the Activity (generated internally by the database).
		/// </summary>
		[Display(Name = "Activity Id")]
		//[Required]
		[Range(1, int.MaxValue)]
		public int? Id { get; set; }

		/// <summary>
		/// Name of the athlete doing the Activity.
		/// </summary>
		[Display(Name = "Athlete Name")]
		[Required(AllowEmptyStrings = false)]
		[StringLength(512)]
		public string AthleteName { get; set; }

		/// <summary>
		/// Type of Activity being performed (Walk, Run, Bike, Swim).
		/// </summary>
		[Display(Name = "Activity Type")]
		[Required]
		[Range(0, 3)]
		public ActivityType Type { get; set; }

		/// <summary>
		/// Status of the Activity (Pending, Started, Paused, Complete).
		/// </summary>
		[Display(Name = "Activity Status")]
		[Required]
		[Range(0, 3)]
		public ActivityStatus Status { get; set; }

		/// <summary>
		/// Time the Activity object was created in the system.
		/// </summary>
		[Display(Name = "Created At")]
		[Required]
		[DataType(DataType.DateTime)]
		// https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/validation
		//[Range(typeof(DateTime), "1/1/1900", "12/31/2050", ErrorMessage = "Value for {0} must be between {1} and {2}")]
		public DateTime CreatedAt { get; set; }

		/// <summary>
		/// Start time for this Activity.
		/// </summary>
		[Display(Name = "Started At")]
		[DataType(DataType.DateTime)]
		//[Range(typeof(DateTime), "1/1/1900", "12/31/2050", ErrorMessage = "Value for {0} must be between {1} and {2}")]
		public DateTime? StartedAt { get; set; }

		/// <summary>
		/// End time for this Activity.
		/// </summary>
		[Display(Name = "Completed At")]
		[DataType(DataType.DateTime)]
		//[Range(typeof(DateTime), "1/1/1900", "12/31/2050", ErrorMessage = "Value for {0} must be between {1} and {2}")]
		public DateTime? CompletedAt { get; set; }

		/// <summary>
		/// Description containing extra details about the Activity.
		/// </summary>
		[Required(AllowEmptyStrings = true)]
		[StringLength(1024)]
		public string Description { get; set; }

		/// <summary>
		/// System of measurement units used for this Activity (i.e. English or Metric).
		/// </summary>
		[Display(Name = "Distance Units")]
		[Required]
		[Range(0,1)]
		public DistanceUnits Units { get; set; }

		/// <summary>
		/// Collection of Position coordinates that belong to this Activity.
		/// </summary>
		public IList<PositionDto> Positions { get; set; }

		/// <summary>
		/// Number of Position objects that belong to this Activity.
		/// Note: this is an optimization property so we don't have to map Position children to each Activity,
		/// but we can still display the count.  This will be used when retrieving a list of Activities,
		/// but not when getting a single Activity.
		/// TODO: fix this later with ViewModel.
		/// </summary>
		[Display(Name = "Position Count")]
		[Range(0, int.MaxValue)]
		public int? PositionCount { get; set; }

		public ActivityDto()
		{
			Id = null;
			AthleteName = string.Empty;
			Type = ActivityType.Undefined;
			Status = ActivityStatus.Undefined;
			CreatedAt = DateTime.MinValue;
			StartedAt = null;
			CompletedAt = null;
			Description = string.Empty;
			Units = DistanceUnits.Undefined;
			Positions = new List<PositionDto>();
			PositionCount = null;
		}
	}
}
