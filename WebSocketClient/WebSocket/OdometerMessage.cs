using System.Text.Json.Serialization;
namespace WebSocketClient.WebSocket;

[Serializable]
public sealed class OdometerMessage
{
    [JsonPropertyName("operation")]
    public string? Operation { get; set; }

    [JsonPropertyName("value")]
    public float? Value { get; set; }

    [JsonPropertyName("odometer")]
    public float? Odometer { get; set; }

    [JsonPropertyName("status")]
    public bool? Status { get; set; }
}