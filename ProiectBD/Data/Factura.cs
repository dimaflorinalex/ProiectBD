namespace ProiectBD.Data
{
    public class Factura
    {
        public static class Columns
        {
            public const string Id = "ID";
            public const string IdContract = "ID_CONTRACT";
            public const string DataEmitere = "DATA_EMITERE";
            public const string DataScadenta = "DATA_SCADENTA";
            public const string Valoare = "VALOARE";

            public static readonly List<string> SortableColumns = new List<string>() { Id, IdContract, DataEmitere, DataScadenta, Valoare };
        }

        required public int Id { get; set; }
        required public int IdContract { get; set; }
        required public DateTime DataEmitere { get; set; }
        required public DateTime DataScadenta { get; set; }
        required public decimal Valoare { get; set; }
    }
}
