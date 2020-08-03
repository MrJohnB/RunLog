using LiteBulb.RunLog.Models;
using LiteBulb.RunLog.Services.Activities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LiteBulb.RunLog.Api.Controllers
{
	/// <summary>
	/// ActivitiesController controller class.
	/// Note: Route Constraints (https://stackoverflow.com/questions/44694658/string-route-constraint)
	/// TODO: Make DTO's for this?
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
		/// A list of Activity objects.
		/// </summary>
		/// <returns>Collection of Activity objects that have been submitted to the service</returns>
		// GET api/v1/activities
		[HttpGet()]
		public async Task<IActionResult> GetActivitiesAsync()
		{
			_logger.LogInformation("GetActivitiesAsync() method start.");

			// Get list of Activity objects in the system
			var getListResponse = await Task.FromResult(_activityService.GetList());

			if (getListResponse.HasErrors)
			{
				var message = $"Error in GetActivitiesAsync() method.  Activity list cannot be displayed.  Error message returned from ActivityService: {getListResponse.ErrorMessage}";
				_logger.LogError(message);
				return NotFound(getListResponse.ErrorMessage);
			}

			var models = getListResponse.Result;

			//if (!models.Any())
			//	return NoContent();

			//TODO: map to DTO

			_logger.LogInformation("GetActivitiesAsync() method end.");

			return Ok(models);
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

			//TODO: map to DTO

			_logger.LogInformation($"GetActivityByIdAsync() method end with Activity id '{id}'.");

			return Ok(model);
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
		public async Task<IActionResult> CreateActivityAsync([FromBody] Activity activity)
		{
			_logger.LogInformation("CreateActivityAsync() method start.");

			// Add Activity object to the database
			var addResponse = await Task.FromResult(_activityService.Add(activity));

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

			//TODO: map to DTO

			var id = addedObject.Id;

			_logger.LogInformation($"CreateActivityAsync() method end with Activity id '{id}'.");

			var actionResult = CreatedAtAction(
				actionName: nameof(GetActivityByIdAsync), // ASP.NET Core 3.0 bug: https://stackoverflow.com/questions/59288259/asp-net-core-3-0-createdataction-returns-no-route-matches-the-supplied-values
														  //controllerName: ControllerContext.ActionDescriptor.ControllerName,
				routeValues: new { id },
				value: addedObject);

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
		public async Task<IActionResult> UpdateActvitityAsync([FromRoute] int id, [FromBody] Activity activity)
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

			//TODO: map to update objcet

			// Update Activity
			var updateResponse = _activityService.Update(activity);

			if (updateResponse.HasErrors)
			{
				var message = $"Error in UpdateActvitityAsync() method.  Cannot update Activity with id '{id}'.  Error message returned from ActivityService: {updateResponse.ErrorMessage}";
				_logger.LogError(message);
				return NotFound(updateResponse.ErrorMessage);
			}

			var updatedObject = updateResponse.Result;

			//TODO: map to DTO

			_logger.LogInformation($"UpdateActvitityAsync() method end with Activity id '{id}'.");

			return Ok(updatedObject);
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
