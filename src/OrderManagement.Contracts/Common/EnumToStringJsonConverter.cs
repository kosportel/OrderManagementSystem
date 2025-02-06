using System.Text.Json.Serialization;
using System.Text.Json;

namespace OrderManagement.Contracts.Common
{
    public class EnumToStringJsonConverter<T> : JsonConverter<T> where T : struct, Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string enumString = reader.GetString();
                if (Enum.TryParse<T>(enumString, true, out var enumValue))
                {
                    return enumValue;
                }
                throw new JsonException($"Unable to convert \"{enumString}\" to Enum {typeof(T)}");
            }
            throw new JsonException($"Unexpected token {reader.TokenType} when parsing enum.");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
