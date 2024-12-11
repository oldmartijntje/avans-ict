using RobotProject.EventHandler;

interface IRobotObject
{
    NewEventHandler EventHandler { get; set; }
    void OnInit(); // the function i run after the constructor has completed.
}
