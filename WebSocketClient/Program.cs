using WebSocketClient.WebSocket;

const string serverUrl = "ws://185.246.65.199:9090/ws";
OdometerController controller = new(serverUrl);

await controller.ConnectAsync();


// Example: Sending an odometer value
float odometerValue = 123.45f;
controller.SendOdometerValue(odometerValue);

// Example: Requesting current odometer value
float currentOdometer = await controller.GetCurrentOdometerAsync();
Console.WriteLine("Current Odometer: " + currentOdometer);

// Example: Requesting random status
(bool randomStatus, float randomOdometer) = await controller.GetRandomStatusAsync();
Console.WriteLine("Random Status: " + randomStatus);
Console.WriteLine("Random Odometer: " + randomOdometer);

// Wait for user input before closing the connection
Console.ReadLine();

controller.Close();