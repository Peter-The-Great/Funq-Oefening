class Gast
{
    public int Rating { get; set; }
    public int Boete { get; set; }
    public DateTime GeboorteDatum { get; init; }
    public void Bezoek()
    {
    }
    public void VIPBezoek(int Level)
    {
        if (Level > 4)
            Rating++;
    }
    public void GeefStraf(string daden)
    {
        foreach (string daad in daden.Split(","))
            switch (daad)
            {
                case "beschadigt":
                    Rating -= 1;
                    Boete += 1;
                    break;
                case "looptDoorGras":
                    Boete += 2;
                    break;
                case "afvalOpDeGrond":
                    Rating += 1;
                    Boete += 3;
                    break;
                default:
                    ;
                    break;
            }
    }
}