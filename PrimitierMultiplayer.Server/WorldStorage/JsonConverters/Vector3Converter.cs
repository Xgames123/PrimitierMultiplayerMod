using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrimitierMultiplayer.Server
{

    public class Vector3Converter : JsonConverter<System.Numerics.Vector3>
    {
        public override System.Numerics.Vector3 Read(ref Utf8JsonReader reader,
            Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            System.Numerics.Vector3 result = new System.Numerics.Vector3();

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

                    case "z":
                        result.Z = (float)reader.GetDouble();
                        break;
                }
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer,
            System.Numerics.Vector3 value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("x", value.X);
            writer.WriteNumber("y", value.Y);
            writer.WriteNumber("z", value.Z);
            writer.WriteEndObject();

        }
    }
}
