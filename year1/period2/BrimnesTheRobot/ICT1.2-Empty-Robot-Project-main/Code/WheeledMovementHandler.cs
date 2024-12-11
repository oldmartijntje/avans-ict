using RobotProject.EventHandler;

class WheeledRobotMovementHandler : IMovementHandler
{
    public NewEventHandler EventHandler { get; set; }
    public WheeledRobotMovementHandler(NewEventHandler eventHandler)
    {
        this.EventHandler = eventHandler;
        this.OnInit();
    }

    public void OnInit()
    {
        Console.WriteLine("Initialised WheeledRobotMovementHandler");
    }

    public void RunTick()
    {
        Console.WriteLine("RunTick");
    }

    public void EmergancyStopMovement()
    {
        Console.WriteLine("EmergancyStop");
    }
}