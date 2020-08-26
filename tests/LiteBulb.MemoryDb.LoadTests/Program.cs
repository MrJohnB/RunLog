using LiteBulb.MemoryDb.LoadTests.Constants;
using LiteBulb.RunLog.Configurations.ConfigSections;
using LiteBulb.RunLog.Models;
using LiteBulb.RunLog.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteBulb.MemoryDb.LoadTests
{
	/// <summary>
	/// Integration tests for the MemoryDb class.
	/// </summary>
	class Program
	{
		// Number of items to add to the database during this load test
		private const int ItemCount = 100000;

		// Database object instances
		private static readonly IDatabaseSettings _databaseSettings;
		private static readonly DatabaseContext _databaseContext;
		//TODO: reference to collection?

		/// <summary>
		/// Constructor: read settings from "config file" (or in this case the ConfigSettings class).
		/// </summary>
		static Program()
		{
			_databaseSettings = new DatabaseSettings()
			{
				ConnectionString = ConfigSettings.ConnectionString,
				DatabaseName = ConfigSettings.DatabaseName,
				ActivityCollectionName = ConfigSettings.ActivityCollectionName,
				PositionCollectionName = ConfigSettings.PositionCollectionName
			};

			// Initialize Database
			_databaseContext = new DatabaseContext(_databaseSettings.DatabaseName);
		}

		/// <summary>
		/// Program main entry point.
		/// </summary>
		static void Main()
		{
			Console.WriteLine("Starting database test..." + Environment.NewLine);

			ExecuteTest();

			Console.WriteLine(Environment.NewLine + "Done with database test.");
			Console.ReadKey();
		}

		/// <summary>
		/// Main function that executes the test logic.
		/// </summary>
		private static void ExecuteTest()
		{
			Console.WriteLine("Starting out with 0 items in database." + Environment.NewLine);

			// Display all items to start with (should be empty)
			DisplayItems(GetAllItems());

			Console.WriteLine($"Adding {ItemCount} items to database." + Environment.NewLine);

			// Generate random items and insert
			for (int i = 0; i < ItemCount; i++)
				InsertItem(GenerateItem());
			
			// Display all items after insert
			DisplayItems(GetAllItems());

			Console.WriteLine($"Updating first item in database." + Environment.NewLine);

			// Get first item and change a field
			var item = GetAllItems().First();
			item.AthleteName = "John Bianchi";
			
			// Update the item
			UpdateItem(item);
			
			// Display all items after update
			DisplayItems(GetAllItems());

			Console.WriteLine($"Deleting first item from database." + Environment.NewLine);

			// Delete the first item
			DeleteItem(item.Id);
			
			// Display all items after delete
			DisplayItems(GetAllItems());

			Console.WriteLine($"Deleting all items from database." + Environment.NewLine);

			// Clear all items
			int deletedItemCount = DeleteAllItems();
			if (deletedItemCount != ItemCount - 1)
				throw new Exception();

			// Display all items after delete all
			DisplayItems(GetAllItems());
		}

		/// <summary>
		/// Display a header row (i.e. column names) followed by a list of row values.
		/// </summary>
		/// <param name="items">Collection of Activity objects to display</param>
		private static void DisplayItems(IEnumerable<Activity> items)
		{
			//TODO: make it use reflection to display properties for any object

			int maxDisplayCount = 10;

			string header =
				$"{nameof(Activity.Id),5}" +
				$"{nameof(Activity.AthleteName),15}" +
				$"{nameof(Activity.Type),8}" +
				$"{nameof(Activity.Status),12}" +
				$"{nameof(Activity.CreatedAt),23}" +
				$"{nameof(Activity.StartedAt),23}" +
				$"{nameof(Activity.CompletedAt),23}" +
				$"{nameof(Activity.Description),18}" +
				$"{nameof(Activity.Positions),12}";
			Console.WriteLine(header);
			Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------");

			int displayCount = 0;

			foreach (var item in items)
			{
				if (++displayCount > maxDisplayCount)
					break;

				string row =
					$"{item.Id,5}" +
					$"{item.AthleteName,15}" +
					$"{item.Type,8}" +
					$"{item.Status,12}" +
					$"{item.CreatedAt,23}" +
					$"{item.StartedAt,23}" +
					$"{item.CompletedAt,23}" +
					$"{item.Description,18}" +
					$"{item.Positions.Count,12}";
				Console.WriteLine(row);
			}

			Console.WriteLine(Environment.NewLine);
		}

		/// <summary>
		/// Insert item into database.
		/// </summary>
		/// <param name="item">Activity object instance</param>
		private static void InsertItem(Activity item)
		{
			var collection = _databaseContext.GetCollection<Activity>(_databaseSettings.ActivityCollectionName);
			collection.Insert(item);
		}

		/// <summary>
		/// Update item in database.
		/// </summary>
		/// <param name="item">Activity object instance</param>
		private static void UpdateItem(Activity item)
		{
			var collection = _databaseContext.GetCollection<Activity>(_databaseSettings.ActivityCollectionName);
			collection.Update(item);
		}

		/// <summary>
		/// Delete item from database.
		/// </summary>
		/// <param name="id">Id of Activity object in database</param>
		private static void DeleteItem(int id)
		{
			var collection = _databaseContext.GetCollection<Activity>(_databaseSettings.ActivityCollectionName);
			collection.Delete(id);
		}

		/// <summary>
		/// Get all items currently in database.
		/// </summary>
		/// <returns>Collection of Activity objects</returns>
		private static IEnumerable<Activity> GetAllItems()
		{
			var collection = _databaseContext.GetCollection<Activity>(_databaseSettings.ActivityCollectionName);
			return collection.FindAll();
		}

		/// <summary>
		/// Delete all Activity items from database.
		/// </summary>
		/// <returns>Count of the documents successfully deleted from database</returns>
		private static int DeleteAllItems()
		{
			var collection = _databaseContext.GetCollection<Activity>(_databaseSettings.ActivityCollectionName);
			return collection.DeleteAll();
		}

		/// <summary>
		/// Generate an object instance with random field values.
		/// </summary>
		/// <returns>Activity object instance</returns>
		private static Activity GenerateItem()
		{
			var random = new Random();
			var type = (ActivityType)random.Next(0, Enum.GetValues(typeof(ActivityType)).Length - 1);
			var status = (ActivityStatus)random.Next(0, Enum.GetValues(typeof(ActivityStatus)).Length - 1);
			var name = $"Name {random.Next(100, 999)}";
			var description = $"Description {random.Next(100, 999)}";

			var item = new Activity()
			{
				AthleteName = name,
				Type = type,
				Status = status,
				CreatedAt = DateTime.Now,
				StartedAt = DateTime.Now,
				CompletedAt = DateTime.Now,
				Description = description
			};

			return item;
		}
	}
}
