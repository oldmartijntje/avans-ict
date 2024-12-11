
using RobotProject.EventHandler;

class RobotClass : IRobotObject
{
    public NewEventHandler EventHandler { get; set; }
    private int TickId { get; set; }
    private IMovementHandler MovementHandler { get; set; }
    public RobotClass(NewEventHandler eventHandler, IMovementHandler movementHandler)
    {
        this.MovementHandler = movementHandler;
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
        if (this.TickId % RobotConfig.ROBOT_MOVEMENT_TICK_INTERVAL == 0)
        {
            this.MovementHandler.RunTick();
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