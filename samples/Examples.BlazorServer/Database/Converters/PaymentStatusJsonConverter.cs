using System.Text.Json;
using System.Text.Json.Serialization;

namespace Examples.BlazorServer.Database.Converters;

public sealed class PaymentStatusJsonConverter : JsonConverter<PaymentStatus>
{
    public override PaymentStatus Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString()!;
        }

        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        if (jsonDoc.RootElement.ValueKind == JsonValueKind.Object &&
            jsonDoc.RootElement.TryGetProperty(nameof(PaymentStatus.Name), out var nameElement) &&
            nameElement.ValueKind == JsonValueKind.String)
        {
            return nameElement.GetString()!;
        }

        throw new JsonException("Invalid payment status");
    }

    public override void Write(Utf8JsonWriter writer, PaymentStatus value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Name);
    }
}