using Microsoft.AspNetCore.Mvc.RazorPages;
using Oracle.ManagedDataAccess.Client;
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
                                        IdReclamatie = reader.GetInt32(reader.GetOrdinal(VizualizareReclamatie.Columns.IdReclamatie)),
                                        Informatii = reader.GetString(reader.GetOrdinal(VizualizareReclamatie.Columns.Informatii)),
                                        DataInregistrare = reader.GetDateTime(reader.GetOrdinal(VizualizareReclamatie.Columns.DataInregistrare)),
                                        DataSolutionare = reader.IsDBNull(reader.GetOrdinal(VizualizareReclamatie.Columns.DataSolutionare)) ? null : reader.GetDateTime(reader.GetOrdinal(VizualizareReclamatie.Columns.DataSolutionare)),
                                        IdClient = reader.GetInt32(reader.GetOrdinal(VizualizareReclamatie.Columns.IdClient)),
                                        Nume = reader.GetString(reader.GetOrdinal(VizualizareReclamatie.Columns.Nume)),
                                        Email = reader.GetString(reader.GetOrdinal(VizualizareReclamatie.Columns.Email)),
                                        Telefon = reader.GetString(reader.GetOrdinal(VizualizareReclamatie.Columns.Telefon)),
                                        TipPersoana = Enum.Parse<TipPersoana>(reader.GetString(reader.GetOrdinal(VizualizareReclamatie.Columns.TipPersoana)))
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