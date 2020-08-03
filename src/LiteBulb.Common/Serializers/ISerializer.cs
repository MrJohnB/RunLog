
namespace LiteBulb.Common.Serializers
{
	/// <summary>
	/// Interface for a Serializer class.
	/// </summary>
	public interface ISerializer
	{
		/// <summary>
		/// Create a string serialized representation of an object.
		/// </summary>
		/// <typeparam name="T">The type of the object to serialize (generic)</typeparam>
		/// <param name="toSerialize">The object instance to serialize</param>
		/// <returns>String representation of the object that was serialized</returns>
		string Serialize<T>(T toSerialize);

		/// <summary>
		/// Create an object instance from a string serialized representation of that object.
		/// </summary>
		/// <typeparam name="T">The type of the object to deserilize (generic)</typeparam>
		/// <param name="toDeserialize">The string representation of the object to deserialize</param>
		/// <returns>The object instance of the string that was deserialized</returns>
		T Deserialize<T>(string toDeserialize);
	}
}
