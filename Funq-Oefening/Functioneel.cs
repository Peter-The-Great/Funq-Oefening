using System.Collections.Immutable;
namespace Functioneel
{
    record Bezoek
    {
        public DateTime Datum { get; init; }
        public Bezoek(DateTime datum)
        {
            Datum = datum;
        }
        public Bezoek() : this(DateTime.Now)
        {
            Datum = DateTime.Now;
        }
    }
    record VIPBezoek : Bezoek
    {
        public int Level { get; init; }
        public VIPBezoek(int Level)
        {
            this.Level = Level;
        }
        public bool VIPBezoekVoorwaarde { get; init; }
        //Je kunt deze opdracht interessanter maken op de volgende manier: 

        public static List<(Func<VIPBezoek, bool>, int)> PuntenSysteem = new List<(Func<VIPBezoek, bool>, int)>()
        {
            (v => v.Level > 4, 2)
        };
    }
    readonly record struct GastStatus
    {
        public int Rating { get; init; }
        public int Boete { get; init; }
        public GastStatus(int rating, int boete)
        {
            Rating = rating;
            Boete = boete;
        }
        public GastStatus VerhoogBoete(int b) => this with { Boete = Boete + b };
        public GastStatus VerhoogRating(int r) => this with { Rating = Rating + r };
        private delegate GastStatus BewerkGastStatus(GastStatus g);
        private static BewerkGastStatus Straf(int boeteD, int ratingD) =>
            (GastStatus s) => s.VerhoogBoete(boeteD).VerhoogRating(ratingD);
        private static BewerkGastStatus Gratie() =>
            (GastStatus s) => s.ScheldBoeteKwijt();
        private static BewerkGastStatus Straf(string daad) => daad switch
        {
            "beschadigt" => Straf(1, -1),
            "looptDoorGras" => Straf(2, 0),
            "afvalOpDeGrond" => Straf(3, -1),
            _ => (x) => x
        };
        public GastStatus MetStraf(string tekst) => TotaleStraf(tekst.Split(","))(this);
        public GastStatus ScheldBoeteKwijt() => this with { Boete = 0 };

        private BewerkGastStatus TotaleStraf(string[] straf) =>
            straf switch
            {
                { Length: 0 } => (GastStatus s) => s,
                _ => Straf(straf[0]) + TotaleStraf(straf[1..])
            };
    }
    record Gast
    {
        private GastStatus GastStatus => new GastStatus().MetStraf(Straffen);
        public int Rating => MijnVIPBezoekjes().Count + GastStatus.Rating;
        public int Boete => GastStatus.Boete;
        private string Straffen { get; init; } = "";
        public DateTime GeboorteDatum { get; init; }
        public IImmutableList<Bezoek> Bezoekjes { get; init; } = ImmutableList<Bezoek>.Empty;
        private Gast Bezoek(Bezoek b) => this with { Bezoekjes = Bezoekjes.Add(b) };
        public Gast Bezoek() => Bezoek(new Bezoek());
        public Gast VIPBezoek(int level) => Bezoek(new VIPBezoek(level));
        private IImmutableList<Bezoek> MijnVIPBezoekjes(IImmutableList<Bezoek> bezoekjes) =>
            bezoekjes switch
            {
                [] => ImmutableList<Bezoek>.Empty,
                [VIPBezoek { VIPBezoekVoorwaarde: true } v, ..] => MijnVIPBezoekjes(bezoekjes.RemoveAt(0)).Add(bezoekjes[0]),
                _ => MijnVIPBezoekjes(bezoekjes.RemoveAt(0))
            };
        public IImmutableList<Bezoek> MijnVIPBezoekjes() => MijnVIPBezoekjes(Bezoekjes);
        public Gast GeefStraf(string straf) => this with { Straffen = Straffen + "," + straf };
        public static Func<VIPBezoek, bool> VIPBezoekVoorwaarde = (b) => b.Level > 4;
    }
}