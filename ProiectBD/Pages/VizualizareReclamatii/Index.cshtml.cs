using Microsoft.AspNetCore.Mvc.RazorPages;
using Oracle.ManagedDataAccess.Client;
using ProiectBD.Data;
using ProiectBD.Dto;
using ProiectBD.Enums;
using System.Text;

namespace ProiectBD.Pages.VizualizareReclamatii
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        public List<VizualizareReclamatie> Reclamatii { get; set; } = new List<VizualizareReclamatie>();

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void OnGet()
        {
            using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("DataContext")))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;

                        StringBuilder query = new StringBuilder("SELECT * FROM V_RECLAMATII");

                        cmd.CommandText = query.ToString();

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Reclamatii.Add(new VizualizareReclamatie()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal(VizualizareReclamatie.Columns.Id)),
                                        IdClient = reader.GetInt32(reader.GetOrdinal(VizualizareReclamatie.Columns.IdClient)),
                                        Informatii = reader.GetString(reader.GetOrdinal(VizualizareReclamatie.Columns.Informatii)),
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