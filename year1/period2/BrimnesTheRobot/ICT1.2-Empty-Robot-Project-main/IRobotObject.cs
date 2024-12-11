using RobotProject.EventHandler;

interface IRobotObject
{
    NewEventHandler EventHandler { get; set; }
    void OnInit();
}
