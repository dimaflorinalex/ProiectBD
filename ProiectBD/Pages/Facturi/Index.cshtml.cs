using Microsoft.AspNetCore.Mvc.RazorPages;
using Oracle.ManagedDataAccess.Client;
using ProiectBD.Data;
using ProiectBD.Enums;
using System.Text;

namespace ProiectBD.Pages.Facturi
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        public List<Factura> Facturi { get; set; } = new List<Factura>();
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

                        StringBuilder query = new StringBuilder("SELECT * FROM FACTURI");
                        if (!string.IsNullOrWhiteSpace(orderBy) && Factura.Columns.SortableColumns.Contains(orderBy))
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
                                Facturi.Add(new Factura()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal(Factura.Columns.Id)),
                                        IdContract = reader.GetInt32(reader.GetOrdinal(Factura.Columns.IdContract)),
                                        DataEmitere = reader.GetDateTime(reader.GetOrdinal(Factura.Columns.DataEmitere)),
                                        DataScadenta = reader.GetDateTime(reader.GetOrdinal(Factura.Columns.DataScadenta)),
                                        Valoare = reader.GetDecimal(reader.GetOrdinal(Factura.Columns.Valoare))
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