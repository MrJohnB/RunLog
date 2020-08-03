using System;
//using System.Text.Json;

namespace LiteBulb.Common.Serializers
{
	/// <summary>
	/// System.Text.Json (Newest and fewest dependencies for .NET Core).
	/// https://michaelscodingspot.com/the-battle-of-c-to-json-serializers-in-net-core-3/
	/// </summary>
	public class TextJsonSerializer : ISerializer
	{
		/// <summary>
		/// Gets an instance of TextJsonSerializer object.
		/// Note: Not a singleton pattern, this is just a convenience method.
		/// </summary>
		/// <returns>Instance of TextJsonSerializer</returns>
		public static TextJsonSerializer GetInstance()
		{
			return new TextJsonSerializer();
		}

		/// <summary>
		/// Create a string serialized representation of an object.
		/// </summary>
		/// <typeparam name="T">The type of the object to serialize (generic)</typeparam>
		/// <param name="toSerialize">The object instance to serialize</param>
		/// <returns>String representation of the object that was serialized</returns>
		public string Serialize<T>(T toSerialize)
		{
			//string jsonString = JsonSerializer.Serialize(toSerialize);
			//return jsonString;

			throw new NotImplementedException();
		}

		/// <summary>
		/// Create an object instance from a string serialized representation of that object.
		/// </summary>
		/// <typeparam name="T">The type of the object to deserilize (generic)</typeparam>
		/// <param name="toDeserialize">The string representation of the object to deserialize</param>
		/// <returns>The object instance of the string that was deserialized</returns>
		public T Deserialize<T>(string toDeserialize)
		{
			throw new NotImplementedException();
		}
	}
}
