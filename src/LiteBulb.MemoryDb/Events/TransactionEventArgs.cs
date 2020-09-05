using System;

namespace LiteBulb.MemoryDb.Events
{
	public class TransactionEventArgs : EventArgs
	{
		public Transaction Transaction { get; }

		public TransactionEventArgs(Transaction transaction)
		{
			Transaction = transaction;
		}
	}
}