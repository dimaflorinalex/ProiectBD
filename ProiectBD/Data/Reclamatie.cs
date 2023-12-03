namespace ProiectBD.Data
{
    public class Reclamatie
    {
        public static class Columns
        {
            public const string Id = "ID";
            public const string IdClient = "ID_CLIENT";
            public const string Informatii = "INFORMATII";
            public const string DataInregistrare = "DATA_INREGISTRARE";
            public const string DataSolutionare = "DATA_SOLUTIONARE";

            public static readonly List<string> SortableColumns = new List<string>() { Id, IdClient, Informatii, DataInregistrare, DataSolutionare };
        }

        required public int Id { get; set; }
        required public int IdClient { get; set; }
        required public string Informatii { get; set; }
        required public DateTime DataInregistrare { get; set; }
        required public DateTime? DataSolutionare { get; set; }
    }
}
