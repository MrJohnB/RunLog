using LiteBulb.MemoryDb.Events;

namespace LiteBulb.MemoryDb.Logging
{
	public interface ITransactionLogger
	{
		bool Enabled { get; set; }
		void OnTransactionOccurred(object source, TransactionEventArgs args);
	}
}
