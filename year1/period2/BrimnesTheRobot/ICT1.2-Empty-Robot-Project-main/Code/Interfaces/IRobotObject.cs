using RobotProject.EventHandler;

interface IRobotObject
{
    // almost all objects are an IRobotObject
    // this is the base interface for all objects in the robot
    NewEventHandler EventHandler { get; set; }
    void OnInit(); // the function i run after the constructor has completed.
}
