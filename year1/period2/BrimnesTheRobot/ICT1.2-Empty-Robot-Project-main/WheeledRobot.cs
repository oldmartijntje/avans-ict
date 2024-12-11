
using RobotProject.EventHandler;

class WheeledRobot : IRobotObject
{
    public NewEventHandler EventHandler { get; set; }
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
}