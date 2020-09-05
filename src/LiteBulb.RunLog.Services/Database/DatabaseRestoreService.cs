using System;
using System.IO;
using System.Text.Json;
using LiteBulb.Common.DataModel;
using LiteBulb.MemoryDb;
using LiteBulb.MemoryDb.Events;
using LiteBulb.RunLog.Models;

namespace LiteBulb.RunLog.Services.Database
{
	/// <summary>
	/// Restore service reads the database transaction log file to restore data back to the database.
	/// Note: this class is not in the MemoryDb project because it is coupled to application specific data model objects.
	/// </summary>
	public class DatabaseRestoreService
	{
		private const string ActivityCollectionName = nameof(Activity);
		private const string PositionCollectionName = nameof(Position);

		private readonly DatabaseContext _databaseContext;
		private readonly IMemoryCollection<Activity> _activityCollection;
		private readonly IMemoryCollection<Position> _positionCollection;

		/// <summary>
		/// Constructor for RestoreService.
		/// </summary>
		/// <param name="databaseContext">DatabaseContext instance</param>
		public DatabaseRestoreService(DatabaseContext databaseContext)
		{
			_databaseContext = databaseContext;
			_activityCollection = databaseContext.Database.GetCollection<Activity>(ActivityCollectionName);
			_positionCollection = databaseContext.Database.GetCollection<Position>(PositionCollectionName);
		}

		/// <summary>
		/// Read a database transation log file and restore data back into MemoryDb database.
		/// </summary>
		/// <param name="transactionLogFilePath">Path to transaction log file</param>
		public async void RestoreFromTransactionLog(string transactionLogFilePath)
		{
			if (string.IsNullOrWhiteSpace(transactionLogFilePath))
				throw new ArgumentNullException();

			if (!File.Exists(transactionLogFilePath))
				throw new Exception($"File does not exist: '{transactionLogFilePath}'.");

			_databaseContext.Database.SetTransactionLoggingEnablement(false);

			using (var streamReader = new StreamReader(transactionLogFilePath))
			{
				while (streamReader.Peek() >= 0)
				{
					var json = await streamReader.ReadLineAsync();
					var transaction = JsonSerializer.Deserialize<Transaction>(json);
					SetupTransaction(transaction);
				}
			}

			_databaseContext.Database.SetTransactionLoggingEnablement(true);
		}

		private void SetupTransaction(Transaction transaction)
		{
			foreach (var document in transaction.Documents)
			{
				switch (transaction.CollectionName)
				{
					case ActivityCollectionName:
						var activity = JsonSerializer.Deserialize<Activity>(document.ToString());
						ExecuteTransaction(_activityCollection, transaction.Type, activity);
						break;
					case PositionCollectionName:
						var position = JsonSerializer.Deserialize<Position>(document.ToString());
						ExecuteTransaction(_positionCollection, transaction.Type, position);
						break;
					default:
						throw new ArgumentException();
				}
			}
		}

		private void ExecuteTransaction<TDocument>(IMemoryCollection<TDocument> collection, TransactionType transactionType, TDocument document) where TDocument : BaseModel<int>
		{
			switch (transactionType)
			{
				case TransactionType.Insert:
					collection.Insert(document);
					break;
				case TransactionType.Update:
					collection.Update(document);
					break;
				case TransactionType.Delete:
					collection.Delete(document.Id);
					break;
				default:
					throw new NotSupportedException();
			}
		}
	}
}
