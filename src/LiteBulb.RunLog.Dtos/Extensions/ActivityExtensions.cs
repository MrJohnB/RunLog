
namespace LiteBulb.RunLog.Dtos.Extensions
{
	/// <summary>
	/// Extension methods that provide some convenience methods for ActivityDto objects.
	/// </summary>
	public static class ActivityExtensions
	{

		/// <summary>
		/// Returns a shortened version of the Description property of the ActivityDto POCO.
		/// </summary>
		/// <param name="activityDto">Activity DTO object instance</param>
		/// <param name="length">Number of characters to return for substring (including the "...")</param>
		/// <returns>Shortened substring of the Description property</returns>
		public static string ShortDescription(this ActivityDto activityDto, int length = 20)
		{
			if (string.IsNullOrWhiteSpace(activityDto.Description))
				return string.Empty;

			if (length >= activityDto.Description.Length)
				return activityDto.Description;

			return activityDto.Description.Substring(0, length) + "...";
		}
	}
}
