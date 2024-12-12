class RobotLog
{
    // RobotLog class is used to store log data
    // This logdata is purely for sending through MQTT

    public DateTime Time { get; set; }
    public string Protocol { get; set; }
    public object? Value { get; set; }


    public RobotLog(DateTime time, string protocol, object? value = null)
    {
        this.Time = time;
        this.Protocol = protocol;
        this.Value = value;
    }
}