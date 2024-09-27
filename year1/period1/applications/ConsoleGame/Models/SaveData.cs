class SaveData
{
    private Dictionary<String, int> RoomVisitCount;

    public SaveData()
    {
        RoomVisitCount = new Dictionary<String, int>();
    }

    public int GetRoomVisitCount(string roomName)
    {
        if (!RoomVisitCount.ContainsKey(roomName))
        {
            return 0;
        }
        return RoomVisitCount[roomName];
    }

    public void IncrementRoomVisitCount(string roomName)
    {
        if (RoomVisitCount.ContainsKey(roomName))
        {
            RoomVisitCount[roomName]++;
        }
        else
        {
            RoomVisitCount.Add(roomName, 1);
        }
    }
}