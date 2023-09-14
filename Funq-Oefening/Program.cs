namespace Functioneel;
class Program
{
    public static void Main(string[] args)
    {
        Gast gast = new Gast() { GeboorteDatum = new DateTime(2000, 1, 1) };
        Console.WriteLine(gast.MijnVIPBezoekjes().Count);
        Console.WriteLine(gast.Rating);
    }
}