namespace ProiectBD.Data
{
    public class LocDeConsum
    {
        public static class Columns
        {
            public const string Id = "ID";
            public const string IdClient = "ID_CLIENT";
            public const string Judet = "JUDET";
            public const string Localitate = "LOCALITATE";
            public const string Strada = "STRADA";
            public const string NrStrada = "NUMAR_STRADA";
            public const string CodPostal = "COD_POSTAL";
            public const string Bloc = "BLOC";
            public const string Scara = "SCARA";
            public const string Etaj = "ETAJ";
            public const string Apartament = "APARTAMENT";

            public static readonly List<string> SortableColumns = new List<string>() { Id, IdClient, Judet, Localitate, Strada, NrStrada, CodPostal, Bloc, Scara, Etaj, Apartament };
        }

        required public int Id { get; set; }
        required public int IdClient { get; set; }
        required public string Judet { get; set; }
        required public string Localitate { get; set; }
        required public string Strada { get; set; }
        required public int NrStrada { get; set; }
        required public string CodPostal { get; set; }
        required public string? Bloc { get; set; }
        required public string? Scara { get; set; }
        required public int? Etaj { get; set; }
        required public int? Apartament { get; set; }
    }
}
