using LiteBulb.Common.Serializers;
using LiteBulb.RunLog.Api.LoadTests.ApiClients;
using LiteBulb.RunLog.Api.LoadTests.Constants;
using LiteBulb.RunLog.Models;
using LiteBulb.RunLog.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteBulb.RunLog.Api.LoadTests
{
	/// <summary>
	/// Integration tests for the RunLog REST API controllers.
	/// </summary>
	class Program
	{
		// Count of Activity parent items to create via the API during load test.
		private const int CreateParentCount = 1000;

		// Count of Position child items to initially include when creating a new Activity.
		private const int InitialChildCount = 5;

		// Count of additional Position child items to create via the API during load test.
		private const int CreateChildCount = 995;

		// URI of controllers.
		private static readonly string _activitiesRequestUri;
		private static readonly string _positionsRequestUri;

		// Client used to send and receive data to a Web API.
		private static readonly IApiClient _apiClient;

		// Collection of Activity items that were created during load test.
		private static readonly List<Activity> _parentItems;

		/// <summary>
		/// Constructor: read settings from "config file" (or in this case the ConfigSettings class) and instatiate object instances.
		/// </summary>
		static Program()
		{
			_activitiesRequestUri = string.Concat(ConfigSettings.BaseRequestUri, ConfigSettings.ActivitiesRequestUri);
			_positionsRequestUri = string.Concat(ConfigSettings.BaseRequestUri, ConfigSettings.PositionsRequestUri);

			_apiClient = new ApiClient(new RestHttpClient(ConfigSettings.BaseUrl), NewtonsoftSerializer.GetInstance());

			_parentItems = new List<Activity>();
		}

		/// <summary>
		/// Program main entry point.
		/// </summary>
		static void Main()
		{
			Console.WriteLine("Press any key to start making REST API calls...");
			Console.ReadLine();

			Console.WriteLine($"Start time: '{DateTime.Now}'" + Environment.NewLine);

			ExecuteTestAsync().Wait();

			Console.WriteLine(Environment.NewLine + "Finished making all REST API calls.");
			Console.WriteLine(Environment.NewLine + $"End time: '{DateTime.Now}'");
			Console.ReadLine();
		}

		/// <summary>
		/// Main function that executes the test logic.
		/// </summary>
		private static async Task ExecuteTestAsync()
		{
			// Set pause time between API calls (milliseconds)
			int millisecondsDelay = 1;

			// Display column names
			DisplayHeader();

			for (int i = 0; i < CreateParentCount; i++)
			{
				var activity = await CreateParentAsync();
				await Task.Delay(millisecondsDelay);
				
				await ReadParentAsync(activity.Id);
				await Task.Delay(millisecondsDelay);

				await CreateChildAsync(activity.Id);
				await Task.Delay(millisecondsDelay);

				await UpdateParentAsync(activity);
				await Task.Delay(millisecondsDelay);

				await ReadParentAsync(activity.Id);
				await Task.Delay(millisecondsDelay);

				//Task createParentTask = CreateParentAsync(createParentDelay);
				//Wait all

				_parentItems.Add(activity);
			}

			// Display count of items from the API
			Console.WriteLine();
			await ReadParentCountAsync();
			await ReadChildCountAsync();

			// Delete the items via API
			foreach (var item in _parentItems)
			{
				await DeleteParentAsync(item.Id);
			}

			// Display final count of items from the API
			Console.WriteLine();
			await ReadParentCountAsync();
			await ReadChildCountAsync();
		}

		/// <summary>
		/// Make HTTP request to create parent object.
		/// </summary>
		/// <returns>Created Activity object (with Id from database)</returns>
		private static async Task<Activity> CreateParentAsync()
		{
			var createdActivity = await _apiClient.CreateAsync<Activity>(_activitiesRequestUri, GenerateActivity());

			if (createdActivity == null || createdActivity == default)
				throw new Exception($"Error occurred during an API create {nameof(Activity)} operation.");

			return createdActivity;
		}

		/// <summary>
		/// Make HTTP request to create child object.
		/// </summary>
		/// <param name="activityId">Activity object id</param>
		/// <returns>CompletedTask</returns>
		private static async Task CreateChildAsync(int activityId)
		{
			int millisecondsDelay = 1;

			for (int i = 0; i < CreateChildCount; i++)
			{
				await Task.Delay(millisecondsDelay);

				var position = GeneratePosition();
				position.ActivityId = activityId;
				var createdPosition = await _apiClient.CreateAsync<Position>(_positionsRequestUri, position);

				if (createdPosition == null || createdPosition == default)
					throw new Exception($"Error occurred during an API create {nameof(Position)} operation.");
			}
		}

		/// <summary>
		/// Make HTTP request to read from API and display to console.
		/// </summary>
		/// <param name="activityId">Activity object id</param>
		/// <returns>CompletedTask</returns>
		private static async Task ReadParentAsync(int id)
		{
			var requestUri = $"{_activitiesRequestUri}/{id}";
			var activity = await _apiClient.ReadAsync<Activity>(requestUri);

			if (activity == null || activity == default)
				throw new Exception($"Error occurred during an API read {nameof(Activity)} operation.");

			DisplayItem(activity);
		}

		/// <summary>
		/// Make HTTP request to update objects.
		/// </summary>
		/// <param name="activityId">Activity object instance</param>
		/// <returns>CompletedTask</returns>
		private static async Task UpdateParentAsync(Activity activity)
		{
			var athleteName = $"UPDATED {activity.AthleteName}";
			var status = ActivityStatus.Complete;

			activity.AthleteName = athleteName;
			activity.Status = status;

			var requestUri = $"{_activitiesRequestUri}/{activity.Id}";
			var updatedActivity = await _apiClient.UpdateAsync<Activity>(requestUri, activity);

			if (updatedActivity == null || updatedActivity == default)
				throw new Exception($"Error occurred during an API update {nameof(Activity)} operation.");

			if (updatedActivity.AthleteName != athleteName || updatedActivity.Status != status)
				throw new Exception("Activity properties not updated correctly following API update operation.");
		}

		/// <summary>
		/// Make HTTP request to delete from API.
		/// </summary>
		/// <param name="activityId">Activity object id</param>
		/// <returns>CompletedTask</returns>
		private static async Task DeleteParentAsync(int id)
		{
			var requestUri = $"{_activitiesRequestUri}/{id}";
			var deleteReponse = await _apiClient.DeleteAsync(requestUri);

			if (string.IsNullOrWhiteSpace(deleteReponse))
				throw new Exception($"Error occurred during an API delete {nameof(Activity)} operation.");
		}

		/// <summary>
		/// Make HTTP request to read final parent object count from API and display to console.
		/// </summary>
		/// <returns>CompletedTask</returns>
		private static async Task ReadParentCountAsync()
		{
			var activities = await _apiClient.ReadAsync<IEnumerable<Activity>>(_activitiesRequestUri);

			int activitiesCount = activities == null ? 0 : activities.Count();

			Console.WriteLine($"Count of activity objects left in service after all API requests: '{activitiesCount}'");
		}

		/// <summary>
		/// Make HTTP request to read final child object count from API and display to console.
		/// </summary>
		/// <returns>CompletedTask</returns>
		private static async Task ReadChildCountAsync()
		{
			var positions = await _apiClient.ReadAsync<IEnumerable<Position>>(_positionsRequestUri);

			int positionsCount = positions == null ? 0 : positions.Count();

			Console.WriteLine($"Count of position objects left in service after all API requests: '{positionsCount}'");
		}

		/// <summary>
		/// Display column names to Console (Activity object).
		/// </summary>
		private static void DisplayHeader()
		{
			//TODO: make it use reflection to display properties for any object
			string header =
				$"{nameof(Activity.Id),5}" +
				$"{nameof(Activity.AthleteName),20}" +
				$"{nameof(Activity.Type),8}" +
				$"{nameof(Activity.Status),12}" +
				$"{nameof(Activity.CreatedAt),23}" +
				$"{nameof(Activity.StartedAt),23}" +
				$"{nameof(Activity.CompletedAt),23}" +
				$"{nameof(Activity.Description),18}" +
				$"{nameof(Activity.Positions),12}";
			Console.WriteLine(header);
			Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------");
		}

		/// <summary>
		/// Display item values to Console.
		/// </summary>
		/// <param name="item">Activity object</param>
		private static void DisplayItem(Activity item)
		{
			//TODO: make it use reflection to display properties for any object
			string row =
				$"{item.Id,5}" +
				$"{item.AthleteName,20}" +
				$"{item.Type,8}" +
				$"{item.Status,12}" +
				$"{item.CreatedAt,23}" +
				$"{item.StartedAt,23}" +
				$"{item.CompletedAt,23}" +
				$"{item.Description,18}" +
				$"{item.Positions.Count,12}";
			Console.WriteLine(row);
		}

		/// <summary>
		/// Generate an Activity object instance with random field values.
		/// </summary>
		/// <returns>Activity object instance</returns>
		private static Activity GenerateActivity()
		{
			var random = new Random();
			var type = (ActivityType)random.Next(0, Enum.GetValues(typeof(ActivityType)).Length - 1);
			var status = (ActivityStatus)random.Next(0, Enum.GetValues(typeof(ActivityStatus)).Length - 2); // Not "Complete"
			var name = $"Name {random.Next(100, 999)}";
			var description = $"Description {random.Next(100, 999)}";

			var activity = new Activity()
			{
				AthleteName = name,
				Type = type,
				Status = status,
				CreatedAt = DateTime.Now,
				StartedAt = DateTime.Now,
				CompletedAt = DateTime.Now,
				Description = description
			};

			// Add a few children if InitialChildCount > 0
			for (int j = 0; j < InitialChildCount; j++)
				activity.Positions.Add(GeneratePosition());

			return activity;
		}

		/// <summary>
		/// Generate a Position object instance with random field values.
		/// </summary>
		/// <returns>Position object instance</returns>
		private static Position GeneratePosition()
		{
			decimal seedLongitude = 0;
			decimal seedLatitude = 0;
			var random = new Random();

			var timeStamp = DateTime.Now;
			var latitude = seedLatitude * random.Next(50, 100) / 100;
			var longitude = seedLongitude * random.Next(50, 100) / 100;
			var altitude = (decimal)random.Next(0, 1000);
			var accuracy = (decimal)random.Next(0, 100);
			var altitudeAccuracy = (decimal)random.Next(0, 100);
			var heading = (decimal)random.Next(0, 360);
			var speed = (decimal)random.Next(0, 360);
			var satelliteCount = random.Next(0, 15);

			var position = new Position()
			{
				TimeStamp = timeStamp,
				Latitude = latitude,
				Longitude = longitude,
				Altitude = altitude,
				Accuracy = accuracy,
				AltitudeAccuracy = altitudeAccuracy,
				Heading = heading,
				Speed = speed,
				SatelliteCount = satelliteCount
			};

			return position;
		}
	}
}
