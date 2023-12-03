using Microsoft.AspNetCore.Mvc.RazorPages;
using Oracle.ManagedDataAccess.Client;
using ProiectBD.Data;
using ProiectBD.Enums;
using System.Text;

namespace ProiectBD.Pages.Reclamatii
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        public List<Reclamatie> Reclamatii { get; set; } = new List<Reclamatie>();
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

                        StringBuilder query = new StringBuilder("SELECT * FROM RECLAMATII");
                        if (!string.IsNullOrWhiteSpace(orderBy) && Reclamatie.Columns.SortableColumns.Contains(orderBy))
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
                                Reclamatii.Add(new Reclamatie()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal(Reclamatie.Columns.Id)),
                                        IdClient = reader.GetInt32(reader.GetOrdinal(Reclamatie.Columns.IdClient)),
                                        Informatii = reader.GetString(reader.GetOrdinal(Reclamatie.Columns.Informatii)),
                                        DataInregistrare = reader.GetDateTime(reader.GetOrdinal(Reclamatie.Columns.DataInregistrare)),
                                        DataSolutionare = reader.IsDBNull(reader.GetOrdinal(Reclamatie.Columns.DataSolutionare)) ? null : reader.GetDateTime(reader.GetOrdinal(Reclamatie.Columns.DataSolutionare))
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