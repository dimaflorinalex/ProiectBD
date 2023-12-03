using ProiectBD.Enums;

namespace ProiectBD.Data
{
    public class Index
    {
        public static class Columns
        {
            public const string Id = "ID";
            public const string IdLocDeConsum = "ID_LOC_DE_CONSUM";
            public const string TipIndex = "TIP_INDEX";
            public const string Data = "DATA";
            public const string Valoare = "VALOARE";

            public static readonly List<string> SortableColumns = new List<string>() { Id, IdLocDeConsum, TipIndex, Data, Valoare };
        }

        required public int Id { get; set; }
        required public int IdLocDeConsum { get; set; }
        required public TipIndex TipIndex { get; set; }
        required public DateTime Data { get; set; }
        required public int Valoare { get; set; }
    }
}
