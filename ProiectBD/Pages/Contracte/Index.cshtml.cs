using Microsoft.AspNetCore.Mvc.RazorPages;
using Oracle.ManagedDataAccess.Client;
using ProiectBD.Data;
using ProiectBD.Enums;
using System.Text;

namespace ProiectBD.Pages.Contracte
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        public List<Contract> Contracte { get; set; } = new List<Contract>();
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

                        StringBuilder query = new StringBuilder("SELECT * FROM CONTRACTE");
                        if (!string.IsNullOrWhiteSpace(orderBy) && Contract.Columns.SortableColumns.Contains(orderBy))
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
                                Contracte.Add(new Contract()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal(Contract.Columns.Id)),
                                        IdLocDeConsum = reader.GetInt32(reader.GetOrdinal(Contract.Columns.IdLocDeConsum)),
                                        TipContract = reader.GetString(reader.GetOrdinal(Contract.Columns.TipContract)),
                                        DataInceput = reader.GetDateTime(reader.GetOrdinal(Contract.Columns.DataInceput)),
                                        DataSfarsit = reader.GetDateTime(reader.GetOrdinal(Contract.Columns.DataSfarsit))
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