
using System.Text.Json;
using Avans.StatisticalRobot;
using RobotProject.EventHandler;

class RobotClass : IRobotObject
{
    public NewEventHandler EventHandler { get; set; }
    private int TickId { get; set; }
    private List<RobotLog> Logs { get; set; }
    private IMovementHandler MovementHandler { get; set; }
    private MqttHandler MqttHandler { get; set; }
    private RobotMode Mode { get; set; }
    private RobotLedButton EmergancyButton { get; set; }
    public RobotClass(NewEventHandler eventHandler, IMovementHandler movementHandler, MqttHandler mqttHandler)
    {
        this.Logs = new List<RobotLog>();
        this.EventHandler = eventHandler;
        this.MovementHandler = movementHandler;
        this.MqttHandler = mqttHandler;
        this.Mode = RobotMode.On;
        this.EmergancyButton = new RobotLedButton(eventHandler, "Emergancy");
        this.EmergancyButton.Led.SetOn();
        this.OnInit();
    }

    private void AppendLog(string message)
    {
        this.Logs.Add(new RobotLog(DateTime.Now, message));
    }

    public void OnInit()
    {
        this.EventHandler.On("ButtonEmergancyPressed", this, (value) =>
        {
            if (this.Mode == RobotMode.EmergancyStop)
            {
                return;
            }
            Console.WriteLine("Emergancy Button Pressed");
            this.AppendLog("Emergancy Button Pressed");
            this.EmergancyButton.Led.SetOff();
            this.Mode = RobotMode.EmergancyStop;
            this.MovementHandler.EmergancyStopMovement();
        });
        this.EventHandler.On("SetRobotMode", this, (value) =>
        {
            if (this.Mode == RobotMode.EmergancyStop)
            {
                return;
            }
            if (!(value is string) || (string)value != "On" && (string)value != "Off")
            {
                Console.WriteLine($"Invalid SetRobotMode value: {value}");
                return;
            }
            if ((string)value == "On")
            {
                this.Mode = RobotMode.On;
            }
            else if ((string)value == "Off")
            {
                this.Mode = RobotMode.Off;
                this.MovementHandler.EmergancyStopMovement();
            }

        });
    }

    public void Tick()
    {
        Console.WriteLine("Tick");
        this.TickId += 1;
        if (this.TickId % RobotConfig.MQTT_REPORT_TICK_INTERVAL == 0)
        {
            this.ReportDataToMqtt();
        }
        if (this.TickId % RobotConfig.ROBOT_BUTTON_CHECK_TICK_INTERVAL == 0)
        {
            this.CheckButtons();
            this.Logs.Add(new RobotLog(DateTime.Now, "Button check"));
        }
        if (this.TickId % RobotConfig.ROBOT_MOVEMENT_TICK_INTERVAL == 0 && this.Mode == RobotMode.On)
        {
            this.MovementHandler.RunTick();
            this.Logs.Add(new RobotLog(DateTime.Now, "Movement tick"));
        }
    }

    private void ReportDataToMqtt()
    {
        Console.WriteLine("MQTT");
        if (this.Logs.Count == 0)
        {
            return;
        }
        List<RobotLog> localLogs = this.Logs;
        this.Logs = new List<RobotLog>();
        string StringifiedLogs = JsonSerializer.Serialize(localLogs, new JsonSerializerOptions
        {
            WriteIndented = true // Makes the JSON pretty-printed (optional)
        });


        // report the data to MQTT
        this.MqttHandler.SendMqttMessage($"{StringifiedLogs}").ContinueWith(task =>
        {
            if (task == null || task.Result == false)
            {
                Console.WriteLine("Error sending MQTT message");
            }
        });
    }

    private void CheckButtons()
    {
        Console.WriteLine("CheckButtons");
        this.EmergancyButton.Check();
    }
}