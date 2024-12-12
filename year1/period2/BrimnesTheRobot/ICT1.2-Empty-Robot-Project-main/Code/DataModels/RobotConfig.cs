static class RobotConfig
{
    // this is the config file for all my hardcoded stuff.
    // this way I can easily change the values without having to go through the code.
    // and it makes it easier to find the values I need to change.

    // Tick intervals / loop logic
    public const int MQTT_REPORT_TICK_INTERVAL = 50;
    public const int ROBOT_BUTTON_CHECK_TICK_INTERVAL = 1;
    public const int ROBOT_MOVEMENT_TICK_INTERVAL = 5;
    public const int LOOP_TICK_MS = 100;
    public const int LOOP_RESET_TICK_INTERVAL = 1000;

    // MQTT
    public const string MQTT_BROKER = "test.mosquitto.org";
    public const int MQTT_PORT = 1883;
    public const bool MQTT_AUTH = false;
    public const string MQTT_USERNAME = "emqxtest";
    public const string MQTT_PASSWORD = "******";
    public const string DEFAULT_MQTT_DATA_SENDING_TOPIC = $"Brimnes/Sending";
    public const string MQTT_DATA_RECEIVING_TOPIC = "Brimnes/Receiving";
    public const bool MQTT_TOPIC_RANDOMISATION = true;
}
