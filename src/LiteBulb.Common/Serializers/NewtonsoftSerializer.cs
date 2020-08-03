using Newtonsoft.Json;

namespace LiteBulb.Common.Serializers
{
	/// <summary>
	/// Json.NET (Recommended - by Newtonsoft).
	/// </summary>
	public class NewtonsoftSerializer : ISerializer
	{
		/// <summary>
		/// Gets an instance of NewtonsoftSerializer object.
		/// Note: Not a singleton pattern, this is just a convenience method.
		/// </summary>
		/// <returns>Instance of NewtonsoftSerializer</returns>
		public static NewtonsoftSerializer GetInstance()
		{
			return new NewtonsoftSerializer();
		}

		/// <summary>
		/// Create a string serialized representation of an object.
		/// </summary>
		/// <typeparam name="T">The type of the object to serialize (generic)</typeparam>
		/// <param name="toSerialize">The object instance to serialize</param>
		/// <returns>String representation of the object that was serialized</returns>
		public string Serialize<T>(T toSerialize)
		{
			// Json.NET
			string jsonString = JsonConvert.SerializeObject(toSerialize, Formatting.Indented);
			return jsonString;
		}

		/// <summary>
		/// Create an object instance from a string serialized representation of that object.
		/// </summary>
		/// <typeparam name="T">The type of the object to deserialize (generic)</typeparam>
		/// <param name="toDeserialize">The string representation of the object to deserialize</param>
		/// <returns>The object instance of the string that was deserialized</returns>
		public T Deserialize<T>(string toDeserialize)
		{
			return JsonConvert.DeserializeObject<T>(toDeserialize);
		}
	}
}
