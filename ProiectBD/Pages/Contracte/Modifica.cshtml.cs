using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oracle.ManagedDataAccess.Client;
using ProiectBD.Data;
using ProiectBD.Enums;

namespace ProiectBD.Pages.Contracte
{
    public class ModificaModel : PageModel
    {
        private readonly ILogger<ModificaModel> _logger;
        private readonly IConfiguration _configuration;

        public Contract? Contract { get; set; }

        public ModificaModel(ILogger<ModificaModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void OnGet(int id)
        {
            using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("DataContext")))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        cmd.CommandText = "SELECT * FROM CONTRACTE WHERE ID = :id AND ROWNUM = 1";
                        cmd.Parameters.Add(new OracleParameter("id", id));

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            Contract = new Contract()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal(Contract.Columns.Id)),
                                IdLocDeConsum = reader.GetInt32(reader.GetOrdinal(Contract.Columns.IdLocDeConsum)),
                                TipContract = reader.GetString(reader.GetOrdinal(Contract.Columns.TipContract)),
                                DataInceput = reader.GetDateTime(reader.GetOrdinal(Contract.Columns.DataInceput)),
                                DataSfarsit = reader.GetDateTime(reader.GetOrdinal(Contract.Columns.DataSfarsit))
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                    }
                }
            }
        }

        public IActionResult OnPost(int id)
        {
            using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("DataContext")))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        cmd.CommandText = "UPDATE CONTRACTE SET ID_LOC_DE_CONSUM = :idLocDeConsum, TIP_CONTRACT = :tipContract, DATA_INCEPUT = :dataInceput, DATA_SFARSIT = :dataSfarsit WHERE ID = :id";
                        cmd.Parameters.Add(new OracleParameter("idLocDeConsum", Request.Form["IdLocDeConsum"][0]));
                        cmd.Parameters.Add(new OracleParameter("tipContract", Request.Form["TipContract"][0]));
                        cmd.Parameters.Add(new OracleParameter("dataInceput", Convert.ToDateTime(Request.Form["DataInceput"][0]).ToString("dd-MM-yyyy")));
                        cmd.Parameters.Add(new OracleParameter("dataSfarsit", Convert.ToDateTime(Request.Form["DataSfarsit"][0]).ToString("dd-MM-yyyy")));
                        cmd.Parameters.Add(new OracleParameter("id", id));

                        cmd.ExecuteNonQuery();

                        return RedirectToPage("/Contracte/Index");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                        return RedirectToPage("/Contracte/Index");
                    }
                }
            }
        }
    }
}