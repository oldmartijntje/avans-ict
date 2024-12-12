class RobotLog
{
    // RobotLog class is used to store log data
    // This logdata is purely for sending through MQTT

    public DateTime Time { get; set; }
    public string Message { get; set; }
    public object? Value { get; set; }


    public RobotLog(DateTime time, string message, object? value = null)
    {
        this.Time = time;
        this.Message = message;
        this.Value = value;
    }
}