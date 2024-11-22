namespace brimnes_webapp.Data;

public class MagicNumberService
{
    public List<int> GetMagicNumberList(int numberOfMagicInts)
    {
        List<int> magicNumbers = [];
        for (int i = 0; i < numberOfMagicInts; i++)
        {
            int number  = Random.Shared.Next(1, 2048);
            magicNumbers.Add(number);
        }
        return magicNumbers;
    }
    
    
}