using Microsoft.AspNetCore.Mvc.RazorPages;
using Oracle.ManagedDataAccess.Client;
using ProiectBD.Data;
using ProiectBD.Enums;
using System.Text;
using Index = ProiectBD.Data.Index;

namespace ProiectBD.Pages.Indecsi
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        public List<Index> Indecsi { get; set; } = new List<Index>();
        public string? OrderBy { get; set; }
        public string? OrderDir { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void OnGet(string? orderBy, string? orderDir)
        {
            OrderBy = orderBy;
            OrderDir = orderDir;

            using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("DataContext")))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;

                        StringBuilder query = new StringBuilder("SELECT * FROM INDECSI");
                        if (!string.IsNullOrWhiteSpace(orderBy) && Index.Columns.SortableColumns.Contains(orderBy))
                        {
                            query.Append(" ORDER BY ");
                            query.Append(orderBy);

                            HashSet<string> orderDirs = new HashSet<string>() { "ASC", "DESC" };
                            if (!string.IsNullOrWhiteSpace(orderDir) && orderDirs.Contains(orderDir))
                            {
                                query.Append(' ');
                                query.Append(orderDir);
                            }
                        }

                        cmd.CommandText = query.ToString();

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Indecsi.Add(new Index()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal(Index.Columns.Id)),
                                        IdLocDeConsum = reader.GetInt32(reader.GetOrdinal(Index.Columns.IdLocDeConsum)),
                                        TipIndex = Enum.Parse<TipIndex>(reader.GetString(reader.GetOrdinal(Index.Columns.TipIndex))),
                                        Data = reader.GetDateTime(reader.GetOrdinal(Index.Columns.Data)),
                                        Valoare = reader.GetInt32(reader.GetOrdinal(Index.Columns.Valoare))
                                    }
                                );
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                    }
                }
            }
        }
    }
}