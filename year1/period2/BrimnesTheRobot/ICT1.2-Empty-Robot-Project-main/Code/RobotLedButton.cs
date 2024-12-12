using Avans.StatisticalRobot;
using RobotProject.EventHandler;

class RobotLedButton : IRobotObject
{
    public NewEventHandler EventHandler { get; set; }
    public Button Button { get; set; }
    public Led Led { get; set; }
    private bool LastState { get; set; }
    private string ButtonName { get; set; }
    public RobotLedButton(NewEventHandler eventHandler, string buttonName)
    {
        this.EventHandler = eventHandler;
        this.Button = new Button(6);
        this.Led = new Led(5);
        this.ButtonName = buttonName;
    }

    public void OnInit()
    {
        // run the first check after initialisation
        this.Check();
    }

    public string GetButtonName()
    {
        // idk if we'll need this, but it's here
        // for getting the button name
        return this.ButtonName;
    }

    public void Check()
    {
        // logic for emitting event when button press is released
        if (this.Button.GetState() == "Released")
        {
            if (this.LastState == true)
            {
                this.EventHandler.Emit($"Button{this.ButtonName}Pressed", true);
            }
        }
        this.LastState = this.Button.GetState() == "Pressed";
    }
}