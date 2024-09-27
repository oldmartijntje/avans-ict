class Main : GameObject
{
    private Room activeRoom;
    private SaveData saveData;
    public Main() : base()
    {
        saveData = new SaveData();
        activeRoom = new Bedroom(saveData);
        activeRoom.OnEnter();
    }
}