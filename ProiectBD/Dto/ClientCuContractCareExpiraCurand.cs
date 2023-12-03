using ProiectBD.Enums;

namespace ProiectBD.Dto
{
    public class ClientCuContractCareExpiraCurand
    {
        public static class Columns
        {
            public const string Nume = "NUME";
            public const string Email = "EMAIL";
            public const string Telefon = "TELEFON";
            public const string TipPersoana = "TIP_PERSOANA";
            public const string Judet = "JUDET";
            public const string Localitate = "LOCALITATE";
            public const string TipContract = "TIP_CONTRACT";
            public const string DataInceput = "DATA_INCEPUT";
            public const string DataSfarsit = "DATA_SFARSIT";
        }

        required public string Nume { get; set; }
        required public string Email { get; set; }
        required public string Telefon { get; set; }
        required public TipPersoana TipPersoana { get; set; }
        required public string Judet { get; set; }
        required public string Localitate { get; set; }
        required public string TipContract { get; set; }
        required public DateTime DataInceput { get; set; }
        required public DateTime DataSfarsit { get; set; }
    }
}
