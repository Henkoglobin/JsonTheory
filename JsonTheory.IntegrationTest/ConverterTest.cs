using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace JsonTheory.IntegrationTest {
	public class ConverterTest {
		[Theory]
		[JsonFileData("TestData/ConverterTestData.json",
			ConverterTypes = new[] { typeof(CharToByteConverter) }
		)]
		public void ConvertersAreUsed(byte aByte, byte[] byteArray) {
			aByte.Should().Be((byte)'A');

			byteArray.Should().HaveCount("Hello, World".Length);

			Encoding.ASCII.GetString(byteArray).Should().Be("Hello, World");
		}

		private class CharToByteConverter : JsonConverter {
			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
				=> throw new NotImplementedException();

			public override object ReadJson(
				JsonReader reader, Type objectType,
				object existingValue, JsonSerializer serializer
			) {
				if (objectType == typeof(byte)) {
					return StringToByte(reader);
				} else if (objectType == typeof(byte[])) {
					return ReadArray(reader).ToArray();
				}

				throw new JsonReaderException();
			}
			private static IEnumerable<byte> ReadArray(JsonReader reader) {
				while (reader.Read() && reader.TokenType != JsonToken.EndArray) {
					if (reader.TokenType == JsonToken.String) {
						yield return StringToByte(reader);
					}
				}
			}
			private static byte StringToByte(JsonReader reader)
				=> reader.Value is string readerValue && readerValue.Length == 1
					? (byte)readerValue.First()
					: throw new FormatException();

			public override bool CanConvert(Type objectType)
				=> objectType == typeof(byte)
					|| objectType == typeof(byte[]);
		}
	}
}
