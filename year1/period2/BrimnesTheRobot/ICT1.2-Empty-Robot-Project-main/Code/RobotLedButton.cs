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
        this.Check();
    }

    public string GetButtonName()
    {
        return this.ButtonName;
    }

    public void Check()
    {
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