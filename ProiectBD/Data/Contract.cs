namespace ProiectBD.Data
{
    public class Contract
    {
        public static class Columns
        {
            public const string Id = "ID";
            public const string IdLocDeConsum = "ID_LOC_DE_CONSUM";
            public const string TipContract = "TIP_CONTRACT";
            public const string DataInceput = "DATA_INCEPUT";
            public const string DataSfarsit = "DATA_SFARSIT";

            public static readonly List<string> SortableColumns = new List<string>() { Id, IdLocDeConsum, TipContract, DataInceput, DataSfarsit };
        }

        required public int Id { get; set; }
        required public int IdLocDeConsum { get; set; }
        required public string TipContract { get; set; }
        required public DateTime DataInceput { get; set; }
        required public DateTime DataSfarsit { get; set; }
    }
}
