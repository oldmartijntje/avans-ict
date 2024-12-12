class RobotLog
{
    public DateTime Time { get; set; }
    public string Message { get; set; }


    public RobotLog(DateTime time, string message)
    {
        this.Time = time;
        this.Message = message;
    }
}