using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleClan.Utilities
{
	public class DateTimeOffsetConverterUsingDateTimeParse : JsonConverter<DateTimeOffset>
	{
		public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return DateTimeOffset.ParseExact(reader.GetString() ?? "", "yyyyMMddTHHmmss.FFFZ", null, DateTimeStyles.None);
		}

		public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToString());
		}
	}
}
