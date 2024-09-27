
abstract class Room : GameObject
{
    string roomName;
    SaveData saveData;
    public Room(string roomName, SaveData saveData) : base()
    {
        this.roomName = roomName;
        this.saveData = saveData;
    }

    public int GetRoomVisitCount()
    {
        return this.saveData.GetRoomVisitCount(this.roomName);
    }

    public void IncrementRoomVisitCount()
    {
        this.saveData.IncrementRoomVisitCount(this.roomName);
    }

    public abstract void GetSaveData();

    public abstract void OnEnter();


}
