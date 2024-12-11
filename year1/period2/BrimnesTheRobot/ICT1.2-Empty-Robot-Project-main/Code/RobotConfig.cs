static class RobotConfig
{
    public const int MQTT_REPORT_TICK_INTERVAL = 50;
    public const int ROBOT_BUTTON_CHECK_TICK_INTERVAL = 1;
    public const int ROBOT_MOVEMENT_TICK_INTERVAL = 5;
    public const int LOOP_TICK_MS = 100;


    public const string MQTT_BROKER = "test.mosquitto.org";
    public const int MQTT_PORT = 1883;
    public const bool MQTT_AUTH = false;
    public const string MQTT_USERNAME = "emqxtest";
    public const string MQTT_PASSWORD = "******";
    public const string DEFAULT_MQTT_DATA_SENDING_TOPIC = "Brimnes/Sending";
    public const string MQTT_DATA_RECEIVING_TOPIC = "Brimnes/Receiving";
}
