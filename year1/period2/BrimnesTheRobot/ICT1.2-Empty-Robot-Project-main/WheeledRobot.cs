
using RobotProject.EventHandler;

enum RobotAction
{
    Left,
    Right,
    Forward,
    Backward,
}

class WheeledRobot : IRobotObject
{
    private Action CurrentAction { get; set; }
    public NewEventHandler EventHandler { get; set; }
    public int TickId { get; set; }
    public WheeledRobot(NewEventHandler eventHandler)
    {
        this.EventHandler = eventHandler;
        this.OnInit();
    }

    public void OnInit()
    {
        this.EventHandler.On("EmergancyStop", this, (value) =>
        {
            // stop the robot from moving
            // But, don't stop the code from running
            // this way we cna still talk through MQTT.
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
        }
    }

    private void ReportDataToMqtt()
    {
        Console.WriteLine("MQTT");
        // report the data to MQTT
    }

    private void CheckButtons()
    {
        Console.WriteLine("CheckButtons");
        // check the buttons
    }
}