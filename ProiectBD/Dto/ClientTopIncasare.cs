using ProiectBD.Enums;

namespace ProiectBD.Dto
{
    public class ClientTopIncasare
    {
        public static class Columns
        {
            public const string Id = "ID";
            public const string Nume = "NUME";
            public const string Email = "EMAIL";
            public const string Telefon = "TELEFON";
            public const string TipPersoana = "TIP_PERSOANA";
            public const string Suma = "SUMA";
        }

        required public int Id { get; set; }
        required public string Nume { get; set; }
        required public string Email { get; set; }
        required public string Telefon { get; set; }
        required public TipPersoana TipPersoana { get; set; }
        required public decimal Suma { get; set; }
    }
}
