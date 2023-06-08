using System.Globalization;

using Newtonsoft.Json;

using WebSocketSharp;
namespace WebSocketClient.WebSocket;

public class OdometerController
{
    private readonly WebSocketSharp.WebSocket _webSocket;
    private TaskCompletionSource<float>? _currentOdometerTcs;
    private TaskCompletionSource<(bool, float)>? _randomStatusTcs;

    public OdometerController(string serverUrl)
    {
        _webSocket = new WebSocketSharp.WebSocket(serverUrl);
        _webSocket.OnMessage += WebSocket_OnMessage;
    }

    public async Task ConnectAsync()
    {
        var connectTask = new TaskCompletionSource<bool>();
        _webSocket.OnOpen += (_, _) => connectTask.SetResult(true);
        _webSocket.ConnectAsync();
        await connectTask.Task;
    }

    public void SendMessage(string message)
    {
        _webSocket.Send(message);
    }

    public async Task<float> GetCurrentOdometerAsync()
    {
        SendMessage("{\"operation\": \"getCurrentOdometer\"}");
        _currentOdometerTcs = new TaskCompletionSource<float>();
        return await _currentOdometerTcs.Task;
    }

    public async Task<(bool, float)> GetRandomStatusAsync()
    {
        SendMessage("{\"operation\": \"getRandomStatus\"}");
        _randomStatusTcs = new TaskCompletionSource<(bool, float)>();
        return await _randomStatusTcs.Task;
    }

    public void SendOdometerValue(float value)
    {
        var message = $"{{\"operation\": \"odometer_val\", \"value\": {value.ToString(CultureInfo.InvariantCulture)}}}";
        SendMessage(message);
    }

    public void Close()
    {
        _webSocket.Close();
    }

    private void WebSocket_OnMessage(object? _, MessageEventArgs e)
    {
        var jsonResponse = JsonConvert.DeserializeObject<OdometerMessage>(e.Data);
        if (jsonResponse == null) return;

        switch (jsonResponse.Operation)
        {
            case "currentOdometer":
                if (jsonResponse.Odometer.HasValue)
                {
                    _currentOdometerTcs?.SetResult(jsonResponse.Odometer.Value);
                }
                else
                {
                    _currentOdometerTcs?.SetException(new FormatException("Failed to parse current odometer value."));
                }
                break;
            case "randomStatus":
                if (jsonResponse is { Status: not null, Odometer: not null })
                {
                    _randomStatusTcs?.SetResult((jsonResponse.Status.Value, jsonResponse.Odometer.Value));
                }
                else
                {
                    _randomStatusTcs?.SetException(new FormatException("Failed to parse random status and odometer value."));
                }
                break;
            default:
                var exception = new ArgumentOutOfRangeException(nameof(jsonResponse.Operation), jsonResponse.Operation, "Operation not found");
                _currentOdometerTcs?.SetException(exception);
                _randomStatusTcs?.SetException(exception);
                break;
        }
    }
}