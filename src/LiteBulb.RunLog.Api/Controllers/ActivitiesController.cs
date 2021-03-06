﻿using LiteBulb.RunLog.Dtos;
using LiteBulb.RunLog.Dtos.Extensions;
using LiteBulb.RunLog.Services.Activities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using LiteBulb.Common.DataModel;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LiteBulb.RunLog.Api.Controllers
{
	/// <summary>
	/// ActivitiesController controller class.
	/// Note: Route Constraints (https://stackoverflow.com/questions/44694658/string-route-constraint)
	/// </summary>
	//[Route("api/[controller]")]
	[Route("api/v1/activities")]
	[ApiController]
	public class ActivitiesController : ControllerBase
	{
		private readonly ILogger<ActivitiesController> _logger;
		private readonly IActivityService _activityService;

		/// <summary>
		/// ActivitiesController constructor.
		/// </summary>
		/// <param name="logger">ILogger instance</param>
		/// <param name="actvityService">IActvityService instance</param>
		public ActivitiesController(
			ILogger<ActivitiesController> logger,
			IActivityService actvityService)
		{
			_logger = logger;
			_activityService = actvityService;
		}

		/// <summary>
		/// A PAGED list of Activity objects.
		/// </summary>
		/// <param name="offset">Number of Activity objects to skip before retrieving list. Default is 0. (Optional: omit if default values are acceptable)</param>
		/// <param name="limit">Number of Activity objects to take when retrieving list. Default is 50 and max is 50. (Optional: omit if default values are acceptable)</param>
		/// <returns>Paged collection of Activity objects that have been submitted to the service</returns>
		// GET api/v1/activities
		[HttpGet("{offset:int},{limit:int}")]
		public async Task<IActionResult> GetActivitiesAsync([FromRoute] int? offset, [FromRoute] int? limit)
		{
			_logger.LogInformation("GetActivitiesAsync() method start.");

			int offsetIndex = 0; // Default index
			int limitSize = 50; // Default size limit

			if (offset != null && offset.HasValue && offset.Value > 0)
				offsetIndex = offset.Value;

			if (limit != null && limit.HasValue && limit.Value < 50)
				limitSize = limit.Value;

			// Get list of Activity objects in the system
			var getPagedListResponse = await Task.FromResult(_activityService.GetPagedList(offsetIndex, limitSize));

			if (getPagedListResponse.HasErrors)
			{
				var message = $"Error in GetActivitiesAsync() method.  Activity list cannot be displayed.  Error message returned from ActivityService: {getPagedListResponse.ErrorMessage}";
				_logger.LogError(message);
				return NotFound(getPagedListResponse.ErrorMessage);
			}

			var pagedResult = getPagedListResponse.Result;

			//if (!pagedResult.Data.Any() && pagedResult.Total <= 0)
			//	return NoContent(); // NotFound($"Activity list is empty and total record count is '{pagedResult.Total}'.");

			var models = pagedResult.Data;
			var dtos = models.MapMany();

			var pagedDto = new PagedResult<ActivityDto>()
			{
				Data = (IReadOnlyCollection<ActivityDto>)dtos,
				Total = pagedResult.Total
			};

			_logger.LogInformation("GetActivitiesAsync() method end.");

			return Ok(pagedDto);
		}

		/// <summary>
		/// Get all details about a specific Activity object from the database.
		/// Note 1: Can check this endpoint to get all details about an Activity.
		/// Note 2: Id is a int (could make it a string).
		/// </summary>
		/// <param name="id">Activity id</param>
		/// <returns>Activity object for a given id</returns>
		// GET api/v1/activities/5
		//[HttpGet("{nameIdentifier:regex(^[[a-zA-Z]])}")]
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetActivityByIdAsync([FromRoute] int id)
		{
			_logger.LogInformation($"GetActivityByIdAsync() method start with Activity id '{id}'.");

			// Get Activity by id
			var getByIdResponse = await Task.FromResult(_activityService.GetById(id));

			if (getByIdResponse.HasErrors)
			{
				var message = $"Error in GetActivityByIdAsync() method.  Cannot fetch Activity info.  Error message returned from ActivityService: {getByIdResponse.ErrorMessage}";
				_logger.LogError(message);
				return NotFound(getByIdResponse.ErrorMessage);
			}

			var model = getByIdResponse.Result;
			var dto = model.Map();

			_logger.LogInformation($"GetActivityByIdAsync() method end with Activity id '{id}'.");

			return Ok(dto);
		}

		/// <summary>
		/// Adds a new Activity object to the database.
		/// TODO: the model submitted contains an Id field for this database entity, therefore you could get an MongoWriteException for duplicate key.
		/// </summary>
		/// <param name="activity">Activity object to add</param>
		/// <returns>Location of resource for newly added Activity object with its Id (if POST was successful)</returns>
		/// https://samanthaneilen.github.io/2018/10/09/using-route-constraints.html
		/// https://stackoverflow.com/questions/52922418/the-constraint-reference-slugify-could-not-be-resolved-to-a-type
		// POST api/v1/activities
		//[HttpPost("{activity:Activity}")]
		//[HttpPost("{activity}")]
		[HttpPost()]
		public async Task<IActionResult> CreateActivityAsync([FromBody] ActivityDto activity)
		{
			_logger.LogInformation("CreateActivityAsync() method start.");

			var model = activity.Map();

			// Add Activity object to the database
			var addResponse = await Task.FromResult(_activityService.Add(model));

			if (addResponse.HasErrors)
			{
				if (addResponse.Exception != null)
				{
					var exceptionMessage = $"Error in CreateActivityAsync() method.  Activity object may not have been added to database.  Exception was thrown in ActivityService.  Exception Type: '{addResponse.Exception.GetType()}'.  Exception Message: {addResponse.Exception.Message}";
					_logger.LogError(exceptionMessage);
					return BadRequest(addResponse.ErrorMessage);
				}

				var message = $"Error in CreateActivityAsync() method.  Activity object may not have been added to database.  Error message returned from ActivityService: {addResponse.ErrorMessage}";
				_logger.LogError(message);
				return NotFound(addResponse.ErrorMessage);
			}

			var addedObject = addResponse.Result;
			var dto = addedObject.Map();
			var id = addedObject.Id;

			_logger.LogInformation($"CreateActivityAsync() method end with Activity id '{id}'.");

			var actionResult = CreatedAtAction(
				actionName: nameof(GetActivityByIdAsync), // ASP.NET Core 3.0 bug: https://stackoverflow.com/questions/59288259/asp-net-core-3-0-createdataction-returns-no-route-matches-the-supplied-values
				//controllerName: ControllerContext.ActionDescriptor.ControllerName,
				routeValues: new { id },
				value: dto);

			return actionResult;
		}

		/// <summary>
		/// Update a Activity object by its id.
		/// TODO: use an Update DTO and AutoMapper?
		/// </summary>
		/// <param name="id">Activity id</param>
		/// <param name="activity">Activity update object</param>
		/// <returns>Updated Activity object (full POCO)</returns>
		/// PUT api/v1/activities/5
		[HttpPut("{id:int}")]
		public async Task<IActionResult> UpdateActvitityAsync([FromRoute] int id, [FromBody] ActivityDto activity)
		{
			_logger.LogInformation("UpdateActvitityAsync() method start.");

			if (id != activity.Id)
			{
				var message = $"The submitted id value '{id}' does not match the corresponding field contained in the submitted object Activity id value '{activity.Id}'.";
				_logger.LogError("Error in UpdateActvitityAsync() method.  " + message);
				return BadRequest(message);
			}

			// Get the Activity
			var getByIdResponse = await Task.FromResult(_activityService.GetById(id));

			if (getByIdResponse.HasErrors)
			{
				var message = $"Error in UpdateActvitityAsync() method.  Cannot find Activity with id '{id}'.  Error message returned from ActivityService: {getByIdResponse.ErrorMessage}";
				_logger.LogError(message);
				return NotFound(getByIdResponse.ErrorMessage);
			}

			var model = activity.Map();

			// Update Activity
			var updateResponse = _activityService.Update(model);

			if (updateResponse.HasErrors)
			{
				var message = $"Error in UpdateActvitityAsync() method.  Cannot update Activity with id '{id}'.  Error message returned from ActivityService: {updateResponse.ErrorMessage}";
				_logger.LogError(message);
				return NotFound(updateResponse.ErrorMessage);
			}

			var dto = updateResponse.Result.Map();

			_logger.LogInformation($"UpdateActvitityAsync() method end with Activity id '{id}'.");

			return Ok(dto);
		}

		/// <summary>
		/// Remove a Activity object from the database.
		/// </summary>
		/// <param name="id">Activity id</param>
		/// <returns>200 OK response code and boolean of whether DELETE was successful</returns>
		// DELETE api/v1/activities/5
		[HttpDelete("{id:int}")]
		public IActionResult DeleteActivityById([FromRoute] int id)
		{
			_logger.LogInformation($"DeleteActivityById() method start with Activity id '{id}'.");

			// Delete Activity object from the database
			var removeByIdResponse = _activityService.RemoveById(id);

			if (removeByIdResponse.HasErrors)
			{
				var message = $"Error in DeleteActivityById() method.  Activity object may not have been deleted.  Error message returned from ActivityService: {removeByIdResponse.ErrorMessage}";
				_logger.LogError(message);
				return NotFound(removeByIdResponse.ErrorMessage);
			}

			// Create response message
			var deleteWasSuccessful = removeByIdResponse.Result;
			var responseMessage = deleteWasSuccessful ? "Delete successful" : "Delete unsuccessful";

			_logger.LogInformation($"DeleteActivityById() method end with Activity object id '{id}'.");

			return Ok(responseMessage);
		}
	}
}
