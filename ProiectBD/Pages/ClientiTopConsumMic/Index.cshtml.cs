using Microsoft.AspNetCore.Mvc.RazorPages;
using Oracle.ManagedDataAccess.Client;
using ProiectBD.Data;
using ProiectBD.Dto;
using ProiectBD.Enums;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace ProiectBD.Pages.ClientiTopConsumMic
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        public List<ClientTopConsumMic> Clienti { get; set; } = new List<ClientTopConsumMic>();

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

                        StringBuilder query = new StringBuilder("WITH CONSUM_PE_LC AS (SELECT LC.ID, LC.ID_CLIENT, (MAX(I.VALOARE) - MIN(I.VALOARE)) CONSUM FROM LOCURI_DE_CONSUM LC INNER JOIN INDECSI I ON I.ID_LOC_DE_CONSUM = LC.ID GROUP BY LC.ID, LC.ID_CLIENT HAVING (MAX(I.VALOARE) - MIN(I.VALOARE)) BETWEEN 0 and 500) SELECT CL.*, CLC.CONSUM FROM CLIENTI CL INNER JOIN CONSUM_PE_LC CLC ON CLC.ID_CLIENT = CL.ID WHERE ROWNUM <= 50 ORDER BY CLC.CONSUM DESC");
                        
                        cmd.CommandText = query.ToString();

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Clienti.Add(new ClientTopConsumMic()
                                    {
                                        Nume = reader.GetString(reader.GetOrdinal(ClientTopConsumMic.Columns.Nume)),
                                        Email = reader.GetString(reader.GetOrdinal(ClientTopConsumMic.Columns.Email)),
                                        Telefon = reader.GetString(reader.GetOrdinal(ClientTopConsumMic.Columns.Telefon)),
                                        TipPersoana = Enum.Parse<TipPersoana>(reader.GetString(reader.GetOrdinal(ClientTopConsumMic.Columns.TipPersoana))),
                                        Consum = reader.GetInt32(reader.GetOrdinal(ClientTopConsumMic.Columns.Consum))
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