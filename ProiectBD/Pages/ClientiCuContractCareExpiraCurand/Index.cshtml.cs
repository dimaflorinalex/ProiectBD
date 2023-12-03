using Microsoft.AspNetCore.Mvc.RazorPages;
using Oracle.ManagedDataAccess.Client;
using ProiectBD.Data;
using ProiectBD.Dto;
using ProiectBD.Enums;
using System.Text;

namespace ProiectBD.Pages.ClientiCuContractCareExpiraCurand
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        public List<ClientCuContractCareExpiraCurand> Clienti { get; set; } = new List<ClientCuContractCareExpiraCurand>();

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

                        StringBuilder query = new StringBuilder("SELECT CL.NUME, CL.EMAIL, CL.TELEFON, CL.TIP_PERSOANA, LC.JUDET, LC.LOCALITATE, CO.TIP_CONTRACT, CO.DATA_INCEPUT, CO.DATA_SFARSIT FROM CLIENTI CL " +
                            "INNER JOIN LOCURI_DE_CONSUM LC ON CL.ID = LC.ID_CLIENT " +
                            "INNER JOIN CONTRACTE CO ON LC.ID = CO.ID_LOC_DE_CONSUM " +
                            "WHERE UPPER(LC.JUDET) IN ('BUCURESTI', 'ILFOV') AND ((TO_DATE(CO.DATA_SFARSIT) - TO_DATE(SYSDATE)) <= 30) " +
                            "ORDER BY CO.DATA_SFARSIT");

                        cmd.CommandText = query.ToString();

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Clienti.Add(new ClientCuContractCareExpiraCurand()
                                    {
                                        Nume = reader.GetString(reader.GetOrdinal(ClientCuContractCareExpiraCurand.Columns.Nume)),
                                        Email = reader.GetString(reader.GetOrdinal(ClientCuContractCareExpiraCurand.Columns.Email)),
                                        Telefon = reader.GetString(reader.GetOrdinal(ClientCuContractCareExpiraCurand.Columns.Telefon)),
                                        TipPersoana = Enum.Parse<TipPersoana>(reader.GetString(reader.GetOrdinal(ClientCuContractCareExpiraCurand.Columns.TipPersoana))),
                                        Judet = reader.GetString(reader.GetOrdinal(ClientCuContractCareExpiraCurand.Columns.Judet)),
                                        Localitate = reader.GetString(reader.GetOrdinal(ClientCuContractCareExpiraCurand.Columns.Localitate)),
                                        TipContract = reader.GetString(reader.GetOrdinal(ClientCuContractCareExpiraCurand.Columns.TipContract)),
                                        DataInceput = reader.GetDateTime(reader.GetOrdinal(ClientCuContractCareExpiraCurand.Columns.DataInceput)),
                                        DataSfarsit = reader.GetDateTime(reader.GetOrdinal(ClientCuContractCareExpiraCurand.Columns.DataSfarsit)),
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