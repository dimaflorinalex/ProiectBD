using ProiectBD.Enums;

namespace ProiectBD.Dto
{
    public class VizualizareReclamatie
    {
        public static class Columns
        {
            public const string IdReclamatie = "ID_RECLAMATIE";
            public const string Informatii = "INFORMATII";
            public const string DataInregistrare = "DATA_INREGISTRARE";
            public const string DataSolutionare = "DATA_SOLUTIONARE";
            public const string IdClient = "ID_CLIENT";
            public const string Nume = "NUME";
            public const string Email = "EMAIL";
            public const string Telefon = "TELEFON";
            public const string TipPersoana = "TIP_PERSOANA";
        }

        required public int IdReclamatie { get; set; }
        required public string Informatii { get; set; }
        required public int IdClient { get; set; }
        required public DateTime DataInregistrare { get; set; }
        required public DateTime? DataSolutionare { get; set; }
        required public string Nume { get; set; }
        required public string Email { get; set; }
        required public string Telefon { get; set; }
        required public TipPersoana TipPersoana { get; set; }
    }
}
