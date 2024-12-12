// instructions for the robot movement
enum RobotAction
{
    Left,
    Right,
    Forward,
    Backward,
    Calculate,
    Stop,
}

// the robot mode
// On: communicates with the server and can move
// Off: communicates with the server and does not move, but can be turned on
// EmergancyStop: communicates with the server and does not move, and cannot be turned on
enum RobotMode
{
    On,
    Off,
    EmergancyStop
}