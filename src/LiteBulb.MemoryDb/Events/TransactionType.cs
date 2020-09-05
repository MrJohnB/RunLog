
namespace LiteBulb.MemoryDb.Events
{
	/// <summary>
	/// The type of transaction (i.e. insert, update, delete).
	/// </summary>
	public enum TransactionType
	{
		Undefined = -1,
		Insert = 0,
		Update = 1,
		Delete = 2
	}
}
