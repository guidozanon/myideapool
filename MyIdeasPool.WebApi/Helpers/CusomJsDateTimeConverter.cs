using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MyIdeasPool.WebApi.Helpers
{
	/// <summary>
	/// Converts a <see cref="DateTime"/> to and from a JavaScript <c>Date as milliseconds</c> (e.g. <c>52231943</c>).
	/// </summary>
	public class CustomJsDateTimeConverter : DateTimeConverterBase
	{
		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		/// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
		/// <param name="value">The value.</param>
		/// <param name="serializer">The calling serializer.</param>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			long ticks;

			if (value is DateTime dateTime)
			{
				DateTime utcDateTime = dateTime.ToUniversalTime();
				ticks = (long)(utcDateTime - new DateTime(1970, 1, 1)).TotalMilliseconds;
			}
			else
			{
				throw new JsonSerializationException("Expected date object value.");
			}

			writer.WriteValue(ticks);
		}

		/// <summary>
		/// Reads the JSON representation of the object.
		/// </summary>
		/// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
		/// <param name="objectType">Type of the object.</param>
		/// <param name="existingValue">The existing property value of the JSON that is being converted.</param>
		/// <param name="serializer">The calling serializer.</param>
		/// <returns>The object value.</returns>
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}
