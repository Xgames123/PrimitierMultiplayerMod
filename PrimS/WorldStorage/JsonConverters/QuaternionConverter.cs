using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class QuaternionConverter : JsonConverter<System.Numerics.Quaternion>
{
    public override System.Numerics.Quaternion Read(ref Utf8JsonReader reader,
        Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        System.Numerics.Quaternion result = new System.Numerics.Quaternion();

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

                case "w":
                    result.W = (float)reader.GetDouble();
                    break;
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer,
        System.Numerics.Quaternion value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("x", value.X);
        writer.WriteNumber("y", value.Y);
        writer.WriteNumber("z", value.Z);
        writer.WriteNumber("w", value.W);
        writer.WriteEndObject();
    }
}