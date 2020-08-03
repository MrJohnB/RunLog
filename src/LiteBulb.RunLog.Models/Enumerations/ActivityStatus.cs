
namespace LiteBulb.RunLog.Models.Enumerations
{
	/// <summary>
	/// Status of Activity (i.e. is it paused or complete?).
	/// </summary>
	public enum ActivityStatus
	{
		Undefined = -1,
		Pending = 0,
		Started = 1,
		Paused = 2,
		Complete = 3
	}
}
