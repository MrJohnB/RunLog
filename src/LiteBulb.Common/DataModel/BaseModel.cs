
namespace LiteBulb.Common.DataModel
{
	public abstract class BaseModel<TId>
	{
		/// <summary>
		/// Id of the object (generated internally by the database).
		/// </summary>
		public TId Id { get; set; }
	}
}
