// See https://aka.ms/new-console-template for more information

using System.Text;
using MQTTnet;
using MQTTnet.Protocol;

Console.WriteLine("Hello, World!");

string broker = "test.mosquitto.org";
int port = 1883;
string topic = "AvansDirect/avoidBrightspace";
string clientId = Guid.NewGuid().ToString();
string username = "emqxtest";
string password = "******";
bool loop = true;

// Create a MQTT client factory
var factory = new MqttClientFactory();

// Create a MQTT client instance
var mqttClient = factory.CreateMqttClient();

// Create MQTT client options
var options = new MqttClientOptionsBuilder()
    .WithTcpServer(broker, port) // MQTT broker address and port
    .WithClientId(clientId)
    .WithCleanSession()
    .Build();

var connectResult = await mqttClient.ConnectAsync(options);

if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
{
    Console.WriteLine("Connected to MQTT broker successfully.");

    // Subscribe to a topic
    await mqttClient.SubscribeAsync(topic);

    // Callback function when a message is received
    mqttClient.ApplicationMessageReceivedAsync += e =>
    {
        Console.WriteLine($"Received message: {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
        if (Encoding.UTF8.GetString(e.ApplicationMessage.Payload) == "exit")
        {
            loop = false;
        }
        return Task.CompletedTask;
    };
    for (int i = 0; i < 10; i++)
    {
        var message = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload($"Hello, MQTT! Message number {i}")
            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
            .WithRetainFlag()
            .Build();

        await mqttClient.PublishAsync(message);
        await Task.Delay(1000); // Wait for 1 second
    }

    while (loop)
    {
        await Task.Delay(1000); // Wait for 1 second
    }
        

}
