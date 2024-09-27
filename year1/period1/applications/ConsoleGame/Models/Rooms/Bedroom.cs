class Bedroom : Room
{
    public Bedroom(SaveData saveData) : base("Bedroom", saveData)
    {

    }

    public override void GetSaveData()
    {

    }

    public override void OnEnter()
    {
        if (GetRoomVisitCount() == 0)
        {
            Console.WriteLine("You wake up in your bedroom.");
        }
        else
        {
            Console.WriteLine("You are back in your bedroom.");
        }

        IncrementRoomVisitCount();
    }
}