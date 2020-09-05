using System.Collections.Generic;

namespace LiteBulb.MemoryDb.Events
{
	public class Transaction
	{
		public string CollectionName { get; set; }

		public TransactionType Type { get; set; }

		public IEnumerable<object> Documents { get; set; }

		public Transaction()
		{ }

		public Transaction(string collectionName, TransactionType type, IEnumerable<object> documents)
		{
			CollectionName = collectionName;
			Type = type;
			Documents = documents;
		}
	}
}
