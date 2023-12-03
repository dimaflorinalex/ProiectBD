using ProiectBD.Enums;

namespace ProiectBD.Dto
{
    public class VizualizareReclamatie
    {
        public static class Columns
        {
            public const string Id = "ID";
            public const string IdClient = "ID_CLIENT";
            public const string Informatii = "INFORMATII";
        }

        required public int Id { get; set; }
        required public int IdClient { get; set; }
        required public string Informatii { get; set; }
    }
}
