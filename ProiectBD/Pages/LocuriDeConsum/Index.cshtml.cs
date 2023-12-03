using Microsoft.AspNetCore.Mvc.RazorPages;
using Oracle.ManagedDataAccess.Client;
using ProiectBD.Data;
using ProiectBD.Enums;
using System.Text;

namespace ProiectBD.Pages.LocuriDeConsum
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        public List<LocDeConsum> LocuriDeConsum { get; set; } = new List<LocDeConsum>();
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

                        StringBuilder query = new StringBuilder("SELECT * FROM LOCURI_DE_CONSUM");
                        if (!string.IsNullOrWhiteSpace(orderBy) && LocDeConsum.Columns.SortableColumns.Contains(orderBy))
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
                                LocuriDeConsum.Add(new LocDeConsum()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal(LocDeConsum.Columns.Id)),
                                        IdClient = reader.GetInt32(reader.GetOrdinal(LocDeConsum.Columns.IdClient)),
                                        Judet = reader.GetString(reader.GetOrdinal(LocDeConsum.Columns.Judet)),
                                        Localitate = reader.GetString(reader.GetOrdinal(LocDeConsum.Columns.Localitate)),
                                        Strada = reader.GetString(reader.GetOrdinal(LocDeConsum.Columns.Strada)),
                                        NrStrada = reader.GetInt32(reader.GetOrdinal(LocDeConsum.Columns.NrStrada)),
                                        CodPostal = reader.GetString(reader.GetOrdinal(LocDeConsum.Columns.CodPostal)),
                                        Bloc = reader.IsDBNull(reader.GetOrdinal(LocDeConsum.Columns.Bloc)) ? null : reader.GetString(reader.GetOrdinal(LocDeConsum.Columns.Bloc)),
                                        Scara = reader.IsDBNull(reader.GetOrdinal(LocDeConsum.Columns.Scara)) ? null : reader.GetString(reader.GetOrdinal(LocDeConsum.Columns.Scara)),
                                        Etaj = reader.IsDBNull(reader.GetOrdinal(LocDeConsum.Columns.Etaj)) ? null : reader.GetInt32(reader.GetOrdinal(LocDeConsum.Columns.Etaj)),
                                        Apartament = reader.IsDBNull(reader.GetOrdinal(LocDeConsum.Columns.Apartament)) ? null : reader.GetInt32(reader.GetOrdinal(LocDeConsum.Columns.Apartament))
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