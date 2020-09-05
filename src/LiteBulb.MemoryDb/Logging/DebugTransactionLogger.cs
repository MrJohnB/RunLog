using LiteBulb.MemoryDb.Events;
using System.Diagnostics;
using System.Text.Json;

namespace LiteBulb.MemoryDb.Logging
{
	public class DebugTransactionLogger : ITransactionLogger
	{
		public bool Enabled { get; set; } = true;

		public DebugTransactionLogger()
		{
		}

		public void OnTransactionOccurred(object source, TransactionEventArgs args)
		{
			if (Enabled)
				LogTransaction(args.Transaction);
		}

		protected void LogTransaction(Transaction transaction)
		{
			var json = JsonSerializer.Serialize(transaction);
			Debug.WriteLine(json);
		}
	}
}
