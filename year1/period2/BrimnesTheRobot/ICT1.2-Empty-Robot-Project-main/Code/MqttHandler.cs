
using System.Text;
using MQTTnet;
using MQTTnet.Protocol;

using RobotProject.EventHandler;

class MqttHandler : IRobotObject
{
    private bool activated { get; set; }
    private IMqttClient MqttClient { get; set; }
    public NewEventHandler EventHandler { get; set; }
    private string MqttTopicOffset { get; set; }
    public MqttHandler(NewEventHandler eventHandler)
    {
        this.EventHandler = eventHandler;
        if (RobotConfig.MQTT_TOPIC_RANDOMISATION)
        {
            this.MqttTopicOffset = $"/{Guid.NewGuid().ToString()}";
            Console.WriteLine($"Randomised MQTT topic offset: '{this.MqttTopicOffset}'");
        }
        else
        {
            this.MqttTopicOffset = "";
        }
        this.activated = false;
        this.OnInit();
        // het boeit niet dat er geen await is, zolang er maar niks hieronder komt
    }

    public async Task<bool> SendMqttMessage(string message, string topic = RobotConfig.DEFAULT_MQTT_DATA_SENDING_TOPIC)
    {
        Console.WriteLine($"Sending MQTT message: {message}");
        if (!this.activated)
        {
            return false;
        }
        topic += this.MqttTopicOffset;
        var messageObject = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(message)
                    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                    .WithRetainFlag()
                    .Build();

        await this.MqttClient.PublishAsync(messageObject);
        // await Task.Delay(1000); // Wait for 1 second
        return true;
    }

    public async void OnInit()
    {
        Console.WriteLine("Starting MQTT client...");

        string broker = RobotConfig.MQTT_BROKER;
        int port = RobotConfig.MQTT_PORT;
        string topic = RobotConfig.MQTT_DATA_RECEIVING_TOPIC;
        topic += this.MqttTopicOffset;
        string clientId = Guid.NewGuid().ToString();

        // Create a MQTT client factory
        var factory = new MqttClientFactory();

        // Create a MQTT client instance
        this.MqttClient = factory.CreateMqttClient();
        MqttClientOptions? options = null;
        // Create MQTT client options
        if (RobotConfig.MQTT_AUTH)
        {
            options = new MqttClientOptionsBuilder()
                .WithClientId(clientId)
                .WithTcpServer(broker, port)
                .WithCredentials(RobotConfig.MQTT_USERNAME, RobotConfig.MQTT_PASSWORD)
                .WithCleanSession()
                .Build();
        }
        else
        {
            options = new MqttClientOptionsBuilder()
                .WithTcpServer(broker, port)
                .WithClientId(clientId)
                .WithCleanSession()
                .Build();
        }

        var connectResult = await this.MqttClient.ConnectAsync(options);

        if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
        {
            this.activated = true;
            Console.WriteLine("Connected to MQTT broker successfully.");

            // Subscribe to a topic
            await MqttClient.SubscribeAsync(topic);

            // Callback function when a message is received
            MqttClient.ApplicationMessageReceivedAsync += e =>
            {
                Console.WriteLine($"Received message: {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                this.EventHandler.Emit("rawMqttMessage", Encoding.UTF8.GetString(e.ApplicationMessage.Payload));

                return Task.CompletedTask;
            };
        }
    }
}