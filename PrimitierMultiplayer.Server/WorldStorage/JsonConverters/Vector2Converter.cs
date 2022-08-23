using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrimitierMultiplayer.Server
{
    public class Vector2Converter : JsonConverter<System.Numerics.Vector2>
    {
        public override System.Numerics.Vector2 Read(ref Utf8JsonReader reader,
            Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            System.Numerics.Vector2 result = new System.Numerics.Vector2();


            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return result;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                var propName = reader.GetString();
                reader.Read();
                switch (propName)
                {
                    case "x":
                        result.X = (float)reader.GetDouble();
                        break;

                    case "y":
                        result.Y = (float)reader.GetDouble();
                        break;

                }
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer,
            System.Numerics.Vector2 value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("x", value.X);
            writer.WriteNumber("y", value.Y);
            writer.WriteEndObject();
        }
    }
}

