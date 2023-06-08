using WebSocketClient.WebSocket;
const string serverAddress = "ws://185.246.65.199:9090/ws";
WsClient wsClient = new();
wsClient.MessageReceived += (sender, message) => Console.WriteLine($"Received message: {message}");

await Task.WhenAll(wsClient.ConnectAsync(new Uri(serverAddress)), SendMessages());

async Task SendMessages()
{
    await Task.Delay(2000);
    await wsClient.SendAsync("{\"operation\": \"getCurrentOdometer\"}");

    await Task.Delay(2000);
    await wsClient.SendAsync("{\"operation\": \"getRandomStatus\"}");

    await Task.Delay(2000);
    await wsClient.SendAsync("{\"operation\": \"getRandomStatus\"}");
}


// await client.ConnectAsync(serverUri, CancellationToken.None);
//
// // Отправка запроса на сервер для получения текущего значения одометра
// var odometerRequest = "{\"operation\": \"getCurrentOdometer\"}";
// await SendAsync(client, odometerRequest);
//
// // Получение ответа от сервера
// var odometerResponse = await ReceiveAsync(client);
// Console.WriteLine(odometerResponse);
//
// // Отправка запроса на сервер для получения случайного значения правда/ложь
// var randomStatusRequest = "{\"operation\": \"getRandomStatus\"}";
// await SendAsync(client, randomStatusRequest);
//
// // Получение ответа от сервера
// var randomStatusResponse = await ReceiveAsync(client);
// Console.WriteLine(randomStatusResponse);
//
// // Отправка запроса на сервер для передачи значения одометра
// var odometerValueRequest = "{\"operation\": \"odometer_val\", \"value\": 123.45}";
// await SendAsync(client, odometerValueRequest);
//
// static async Task SendAsync(WebSocket client, string message)
// {
//     var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
//     await client.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
// }
//
// static async Task<string> ReceiveAsync(WebSocket client)
// {
//     var buffer = new ArraySegment<byte>(new byte[8192]);
//     var result = new StringBuilder();
//     WebSocketReceiveResult receiveResult;
//
//     do
//     {
//         receiveResult = await client.ReceiveAsync(buffer, CancellationToken.None);
//         if (buffer.Array != null) result.Append(Encoding.UTF8.GetString(buffer.Array, buffer.Offset, receiveResult.Count));
//     } while (!receiveResult.EndOfMessage);
//
//     return result.ToString();
// }