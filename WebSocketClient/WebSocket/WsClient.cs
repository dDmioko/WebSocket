using System.Net.WebSockets;
using System.Text;
namespace WebSocketClient.WebSocket;

public sealed class WsClient
{
    private ClientWebSocket? _clientWebSocket;

    public event EventHandler<string>? MessageReceived;

    public async Task ConnectAsync(Uri uri)
    {
        _clientWebSocket = new ClientWebSocket();
        await _clientWebSocket.ConnectAsync(uri, CancellationToken.None);
        await ReceiveAsync();
    }

    public async Task SendAsync(string message)
    {
        var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
        if (_clientWebSocket == null) return;
        await _clientWebSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
    }

    private async Task ReceiveAsync()
    {
        var buffer = new byte[1024];
        while (_clientWebSocket?.State == WebSocketState.Open)
        {
            var result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType != WebSocketMessageType.Text) continue;

            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            MessageReceived?.Invoke(this, message);
            Array.Clear(buffer, 0, buffer.Length);
        }
    }
}