namespace ProiectBD.Data
{
    public class Incasare
    {
        public static class Columns
        {
            public const string Id = "ID";
            public const string IdFactura = "ID_FACTURA";
            public const string Data = "DATA";
            public const string Valoare = "VALOARE";

            public static readonly List<string> SortableColumns = new List<string>() { Id, IdFactura, Data, Valoare };
        }

        required public int Id { get; set; }
        required public int IdFactura { get; set; }
        required public DateTime Data { get; set; }
        required public decimal Valoare { get; set; }
    }
}
