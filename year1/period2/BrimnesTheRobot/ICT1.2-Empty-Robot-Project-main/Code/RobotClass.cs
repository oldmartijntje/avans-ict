
using System.Text.Json;
using Avans.StatisticalRobot;
using Newtonsoft.Json;
using RobotProject.EventHandler;

class RobotClass : IRobotObject
{
    // the full robot, this is the main class that handles all the logic
    public NewEventHandler EventHandler { get; set; }
    private int TickId { get; set; }
    private List<RobotLog> Logs { get; set; }
    private IMovementHandler MovementHandler { get; set; }
    private MqttHandler MqttHandler { get; set; }
    private RobotMode Mode { get; set; }
    private RobotLedButton EmergencyButton { get; set; }
    private int? BatteryMillivolts { get; set; }
    public RobotClass(NewEventHandler eventHandler, IMovementHandler movementHandler, MqttHandler mqttHandler)
    {
        this.BatteryMillivolts = null;
        this.Logs = new List<RobotLog>();
        this.EventHandler = eventHandler;
        this.MovementHandler = movementHandler;
        this.MqttHandler = mqttHandler;
        this.Mode = RobotMode.On;
        this.EmergencyButton = new RobotLedButton(eventHandler, "Emergancy");
        this.EmergencyButton.Led.SetOn();
        this.OnInit();
    }

    private void AppendLog(string protocol, object? value = null)
    {
        // add the MQTT log to the logs
        this.Logs.Add(new RobotLog(DateTime.Now, protocol, value));
    }

    public void OnInit()
    {
        // subscribe to all events
        this.EventHandler.On("ButtonEmergancyPressed", this, (value) =>
        {
            if (this.Mode == RobotMode.EmergancyStop)
            {
                return;
            }
            Console.WriteLine("Emergancy Button Pressed");
            this.AppendLog("EmergancyButton", new { Pressed = true });
            this.EmergencyButton.Led.SetOff();
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
            this.AppendLog("SetRobotMode", (string)value);
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

    public async Task Tick()
    {
        // handle all logic in a robottick
        this.TickId += 1;
        // checks per tick to see if X should happen.
        if (this.TickId % RobotConfig.MQTT_REPORT_TICK_INTERVAL == 0)
        {
            var robotInfo = new { TickId = this.TickId, Mode = Enum.GetName(this.Mode), LastMeassuredBatteryMillivolts = this.BatteryMillivolts };
            // Send logs over mqtt
            this.AppendLog("Ping", robotInfo);
            this.ReportDataToMqtt();
            this.AppendLog("PreviousPing", robotInfo);

        }
        if (this.TickId % RobotConfig.ROBOT_BUTTON_CHECK_TICK_INTERVAL == 0)
        {
            // check the emergancy button
            this.CheckButtons();
        }
        if (this.TickId % RobotConfig.ROBOT_MOVEMENT_TICK_INTERVAL == 0 && this.Mode == RobotMode.On)
        {
            // run the movement handler tick
            this.MovementHandler.RunTick();
        }
        if (this.TickId > RobotConfig.LOOP_RESET_TICK_INTERVAL)
        {
            // reset tick id to make sure it doesn't tick over to - values.
            // cause idk if that would break the % operator, probably not but still.
            this.TickId = this.TickId % RobotConfig.LOOP_RESET_TICK_INTERVAL;
        }
    }

    private void ReportDataToMqtt()
    {
        // send all logs to mqtt and empty the logs
        if (this.Logs.Count == 0)
        {
            return;
        }
        List<RobotLog> localLogs = this.Logs;
        this.Logs = new List<RobotLog>();
        string StringifiedLogs = JsonConvert.SerializeObject(localLogs, Formatting.Indented);

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
        try
        {
            this.BatteryMillivolts = Robot.ReadBatteryMillivolts();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error reading battery millivolts.");
        }
        this.EmergencyButton.Check();
    }
}