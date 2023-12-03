using ProiectBD.Enums;

namespace ProiectBD.Dto
{
    public class ClientTopConsumMic
    {
        public static class Columns
        {
            public const string Nume = "NUME";
            public const string Email = "EMAIL";
            public const string Telefon = "TELEFON";
            public const string TipPersoana = "TIP_PERSOANA";
            public const string Consum = "CONSUM";
        }

        required public string Nume { get; set; }
        required public string Email { get; set; }
        required public string Telefon { get; set; }
        required public TipPersoana TipPersoana { get; set; }
        required public int Consum { get; set; }
    }
}
