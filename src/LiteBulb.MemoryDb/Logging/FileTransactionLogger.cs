using LiteBulb.MemoryDb.Events;
using System;
using System.IO;
using System.Text.Json;

namespace LiteBulb.MemoryDb.Logging
{
	public class FileTransactionLogger : ITransactionLogger
	{
		private readonly string _filePath;

		public bool Enabled { get; set; } = true;

		public FileTransactionLogger(string filePath)
		{
			_filePath = filePath ?? throw new ArgumentNullException();

			var directoryPath = Path.GetDirectoryName(filePath);

			if (!Directory.Exists(directoryPath))
				Directory.CreateDirectory(directoryPath);
		}

		public void OnTransactionOccurred(object source, TransactionEventArgs args)
		{
			if (Enabled)
				LogTransaction(args.Transaction);
		}

		protected void LogTransaction(Transaction transaction)
		{
			var json = JsonSerializer.Serialize(transaction);

			using (StreamWriter streamWriter = File.AppendText(_filePath))
			{
				streamWriter.WriteLineAsync(json);
			}
		}
	}
}
