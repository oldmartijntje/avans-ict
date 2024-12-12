class RobotLog
{
    // RobotLog class is used to store log data
    // This logdata is purely for sending through MQTT

    public DateTime Time { get; set; }
    public string ProtocolId { get; set; }
    public object? Value { get; set; }


    public RobotLog(DateTime time, string protocolId, object? value = null)
    {
        this.Time = time;
        this.ProtocolId = protocolId;
        this.Value = value;
    }
}