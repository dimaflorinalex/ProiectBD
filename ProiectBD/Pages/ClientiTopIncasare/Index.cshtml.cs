using Microsoft.AspNetCore.Mvc.RazorPages;
using Oracle.ManagedDataAccess.Client;
using ProiectBD.Dto;
using ProiectBD.Enums;
using System.Text;

namespace ProiectBD.Pages.ClientiTopIncasare
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        public List<ClientTopIncasare> Clienti { get; set; } = new List<ClientTopIncasare>();

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

                        StringBuilder query = new StringBuilder("SELECT * FROM V_TOP_INCASARI_CLIENTI");
                        
                        cmd.CommandText = query.ToString();

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Clienti.Add(new ClientTopIncasare()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal(ClientTopIncasare.Columns.Id)),
                                        Nume = reader.GetString(reader.GetOrdinal(ClientTopIncasare.Columns.Nume)),
                                        Email = reader.GetString(reader.GetOrdinal(ClientTopIncasare.Columns.Email)),
                                        Telefon = reader.GetString(reader.GetOrdinal(ClientTopIncasare.Columns.Telefon)),
                                        TipPersoana = Enum.Parse<TipPersoana>(reader.GetString(reader.GetOrdinal(ClientTopIncasare.Columns.TipPersoana))),
                                        Suma = reader.GetDecimal(reader.GetOrdinal(ClientTopIncasare.Columns.Suma))
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