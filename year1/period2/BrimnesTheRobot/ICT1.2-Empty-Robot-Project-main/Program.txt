using System.Device.Gpio;
using Avans.StatisticalRobot;

Console.WriteLine("Hello world");

Led led5 = new Led(5);
Button button6 = new Button(6);

Led led22 = new Led(22);
Button button23 = new Button(23);

short offset = 1;
bool isOn = true;

Robot.PlayNotes("C4");
Console.WriteLine($"Battery left: {Robot.ReadBatteryMillivolts()}");

string lastState1 = "";
string lastState2 = "";
Console.WriteLine(button6.GetState());
Console.WriteLine(button23.GetState());
while (isOn)
{
    if (button6.GetState() == "Released")
    {
        led5.SetOn();
        if (lastState1 == "Pressed")
        {
            offset = OnClick(offset, isOn);
        }
    }
    else
    {
        led5.SetOff();
    }
    lastState1 = button6.GetState();
    if (button23.GetState() == "Released")
    {
        led22.SetOn();
        Console.WriteLine("Button 23 Released");
        if (lastState2 == "Pressed")
        {
            isOn = false;
            Robot.Wait(200);
            Robot.Motors(0, 0);
            led22.SetOff();
            led5.SetOff();
        }
    }
    else
    {
        led22.SetOff();
    }
    lastState2 = button23.GetState();
    Robot.Wait(200);
}

static short OnClick(short offset, bool isOn)
{
    Console.WriteLine("Button clicked");
    offset = (short)(offset == 1 ? 0 : 1);
    RunBot(offset, isOn);
    // offset -= 1;
    return offset;
}

static void RunBot(short offset, bool isOn)
{
    Robot.PlayNotes("C4C4G4G4A4A4G4F4F4E4E4D4D4C4");
    Robot.Wait(1000);
    Console.WriteLine("Offset: " + offset);

    Robot.Motors((short)(-100 + offset), -100);

    Robot.Wait(15000);

    Robot.Motors(0, 0);


}