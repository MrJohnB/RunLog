using LiteBulb.RunLog.Dtos;
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
	/// PositionsController controller class.
	/// Note: Route Constraints (https://stackoverflow.com/questions/44694658/string-route-constraint)
	/// </summary>
	//[Route("api/[controller]")]
	[Route("api/v1/positions")]
	[ApiController]
	public class PositionsController : ControllerBase
	{
		private readonly ILogger<PositionsController> _logger;
		private readonly IPositionService _positionService;

		/// <summary>
		/// PositionsController constructor.
		/// </summary>
		/// <param name="logger">ILogger instance</param>
		/// <param name="positionService">IPositionService instance</param>
		public PositionsController(
			ILogger<PositionsController> logger,
			IPositionService positionService)
		{
			_logger = logger;
			_positionService = positionService;
		}

		/// <summary>
		/// A PAGED list of Position objects.
		/// </summary>
		/// <param name="offset">Number of Position objects to skip before retrieving list. Default is 0. (Optional: omit if default values are acceptable)</param>
		/// <param name="limit">Number of Position objects to take when retrieving list. Default is 50 and max is 50. (Optional: omit if default values are acceptable)</param>
		/// <returns>Paged collection of Position objects that have been submitted to the service</returns>
		// GET api/v1/positions
		[HttpGet("{offset:int},{limit:int}")]
		public async Task<IActionResult> GetPositionsAsync([FromRoute] int? offset, [FromRoute] int? limit)
		{
			_logger.LogInformation("GetPositionsAsync() method start.");

			int offsetIndex = 0; // Default index
			int limitSize = 50; // Default size limit

			if (offset != null && offset.HasValue && offset.Value > 0)
				offsetIndex = offset.Value;

			if (limit != null && limit.HasValue && limit.Value < 50)
				limitSize = limit.Value;

			// Get list of Position objects in the system
			var getPagedListResponse = await Task.FromResult(_positionService.GetPagedList(offsetIndex, limitSize));

			if (getPagedListResponse.HasErrors)
			{
				var message = $"Error in GetPositionsAsync() method.  Position list cannot be displayed.  Error message returned from PositionService: {getPagedListResponse.ErrorMessage}";
				_logger.LogError(message);
				return NotFound(getPagedListResponse.ErrorMessage);
			}

			var pagedResult = getPagedListResponse.Result;

			//if (!pagedResult.Data.Any() && pagedResult.Total <= 0)
			//	return NoContent(); // NotFound($"Position list is empty and total record count is '{pagedResult.Total}'.");

			var models = pagedResult.Data;
			var dtos = models.MapMany();

			var pagedDto = new PagedResult<PositionDto>()
			{
				Data = (IReadOnlyCollection<PositionDto>)dtos,
				Total = pagedResult.Total
			};

			_logger.LogInformation("GetPositionsAsync() method end.");

			return Ok(pagedDto);
		}

		/// <summary>
		/// Get all details about a specific Position object from the database.
		/// Note 1: Can check this endpoint to get all details about an Position.
		/// Note 2: Id is a int (could make it a string).
		/// </summary>
		/// <param name="id">Position id</param>
		/// <returns>Position object for a given id</returns>
		// GET api/v1/positions/5
		//[HttpGet("{nameIdentifier:regex(^[[a-zA-Z]])}")]
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetPositionByIdAsync([FromRoute] int id)
		{
			_logger.LogInformation($"GetPositionByIdAsync() method start with Position id '{id}'.");

			// Get Position by id
			var getByIdResponse = await Task.FromResult(_positionService.GetById(id));

			if (getByIdResponse.HasErrors)
			{
				var message = $"Error in GetPositionByIdAsync() method.  Cannot fetch Position info.  Error message returned from PositionService: {getByIdResponse.ErrorMessage}";
				_logger.LogError(message);
				return NotFound(getByIdResponse.ErrorMessage);
			}

			var model = getByIdResponse.Result;
			var dto = model.Map();

			_logger.LogInformation($"GetPositionByIdAsync() method end with Position id '{id}'.");

			return Ok(dto);
		}

		/// <summary>
		/// Adds a new Position object to the database.
		/// TODO: the model submitted contains an Id field for this database entity, therefore you could get an MongoWriteException for duplicate key.
		/// </summary>
		/// <param name="position">Position object to add</param>
		/// <returns>Location of resource for newly added Position object with its Id (if POST was successful)</returns>
		/// https://samanthaneilen.github.io/2018/10/09/using-route-constraints.html
		/// https://stackoverflow.com/questions/52922418/the-constraint-reference-slugify-could-not-be-resolved-to-a-type
		// POST api/v1/positions
		//[HttpPost("{position:Position}")]
		//[HttpPost("{position}")]
		[HttpPost()]
		public async Task<IActionResult> CreatePositionAsync([FromBody] PositionDto position)
		{
			_logger.LogInformation("CreatePositionAsync() method start.");

			var model = position.Map();

			// Add Position object to the database
			var addResponse = await Task.FromResult(_positionService.Add(model));

			if (addResponse.HasErrors)
			{
				if (addResponse.Exception != null)
				{
					var exceptionMessage = $"Error in CreatePositionAsync() method.  Position object may not have been added to database.  Exception was thrown in PositionService.  Exception Type: '{addResponse.Exception.GetType()}'.  Exception Message: {addResponse.Exception.Message}";
					_logger.LogError(exceptionMessage);
					return BadRequest(addResponse.ErrorMessage);
				}

				var message = $"Error in CreatePositionAsync() method.  Position object may not have been added to database.  Error message returned from PositionService: {addResponse.ErrorMessage}";
				_logger.LogError(message);
				return NotFound(addResponse.ErrorMessage);
			}

			var addedObject = addResponse.Result;
			var dto = addedObject.Map();
			var id = addedObject.Id;

			_logger.LogInformation($"CreatePositionAsync() method end with Position id '{id}'.");

			var actionResult = CreatedAtAction(
				actionName: nameof(GetPositionByIdAsync), // ASP.NET Core 3.0 bug: https://stackoverflow.com/questions/59288259/asp-net-core-3-0-createdataction-returns-no-route-matches-the-supplied-values
				//controllerName: ControllerContext.ActionDescriptor.ControllerName,
				routeValues: new { id },
				value: dto);

			return actionResult;
		}

		/// <summary>
		/// Update a Position object by its id.
		/// TODO: use an Update DTO and AutoMapper?
		/// </summary>
		/// <param name="id">Position id</param>
		/// <param name="position">Position update object</param>
		/// <returns>Updated Position object (full POCO)</returns>
		/// PUT api/v1/positions/5
		[HttpPut("{id:int}")]
		public async Task<IActionResult> UpdatePositionAsync([FromRoute] int id, [FromBody] PositionDto position)
		{
			_logger.LogInformation("UpdatePositionAsync() method start.");

			if (id != position.Id)
			{
				var message = $"The submitted id value '{id}' does not match the corresponding field contained in the submitted object Position id value '{position.Id}'.";
				_logger.LogError("Error in UpdatePositionAsync() method.  " + message);
				return BadRequest(message);
			}

			// Get the Position
			var getByIdResponse = await Task.FromResult(_positionService.GetById(id));

			if (getByIdResponse.HasErrors)
			{
				var message = $"Error in UpdatePositionAsync() method.  Cannot find Position with id '{id}'.  Error message returned from PositionService: {getByIdResponse.ErrorMessage}";
				_logger.LogError(message);
				return NotFound(getByIdResponse.ErrorMessage);
			}

			var model = position.Map();

			// Update Position
			var updateResponse = _positionService.Update(model);

			if (updateResponse.HasErrors)
			{
				var message = $"Error in UpdatePositionAsync() method.  Cannot update Position with id '{id}'.  Error message returned from PositionService: {updateResponse.ErrorMessage}";
				_logger.LogError(message);
				return NotFound(updateResponse.ErrorMessage);
			}

			var dto = updateResponse.Result.Map();

			_logger.LogInformation($"UpdatePositionAsync() method end with Position id '{id}'.");

			return Ok(dto);
		}

		/// <summary>
		/// Remove a Position object from the database.
		/// </summary>
		/// <param name="id">Position id</param>
		/// <returns>200 OK response code and boolean of whether DELETE was successful</returns>
		// DELETE api/v1/positions/5
		[HttpDelete("{id:int}")]
		public IActionResult DeletePositionById([FromRoute] int id)
		{
			_logger.LogInformation($"DeletePositionById() method start with Position id '{id}'.");

			var removeByIdResponse = _positionService.RemoveById(id);

			if (removeByIdResponse.HasErrors)
			{
				var message = $"Error in DeletePositionById() method.  Position object may not have been deleted.  Error message returned from PositionService: {removeByIdResponse.ErrorMessage}";
				_logger.LogError(message);
				return NotFound(removeByIdResponse.ErrorMessage);
			}

			// Create response message
			var deleteWasSuccessful = removeByIdResponse.Result;
			var responseMessage = deleteWasSuccessful ? "Delete successful" : "Delete unsuccessful";

			_logger.LogInformation($"DeletePositionById() method end with Position object id '{id}'.");

			return Ok(responseMessage);
		}
	}
}
