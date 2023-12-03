using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oracle.ManagedDataAccess.Client;
using ProiectBD.Data;
using ProiectBD.Enums;
using System.Globalization;

namespace ProiectBD.Pages.Facturi
{
    public class ModificaModel : PageModel
    {
        private readonly ILogger<ModificaModel> _logger;
        private readonly IConfiguration _configuration;

        public Factura? Factura { get; set; }

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
                        cmd.CommandText = "SELECT * FROM FACTURI WHERE ID = :id AND ROWNUM = 1";
                        cmd.Parameters.Add(new OracleParameter("id", id));

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            Factura = new Factura()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal(Factura.Columns.Id)),
                                IdContract = reader.GetInt32(reader.GetOrdinal(Factura.Columns.IdContract)),
                                DataEmitere = reader.GetDateTime(reader.GetOrdinal(Factura.Columns.DataEmitere)),
                                DataScadenta = reader.GetDateTime(reader.GetOrdinal(Factura.Columns.DataScadenta)),
                                Valoare = reader.GetDecimal(reader.GetOrdinal(Factura.Columns.Valoare))
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
                        cmd.CommandText = "UPDATE FACTURI SET ID_CONTRACT = :idContract, DATA_EMITERE = :dataEmitere, DATA_SCADENTA = :dataScadenta, VALOARE = :valoare WHERE ID = :id";
                        cmd.Parameters.Add(new OracleParameter("idContract", Request.Form["IdContract"][0]));
                        cmd.Parameters.Add(new OracleParameter("dataEmitere", Convert.ToDateTime(Request.Form["DataEmitere"][0]).ToString("dd-MM-yyyy")));
                        cmd.Parameters.Add(new OracleParameter("dataScadenta", Convert.ToDateTime(Request.Form["DataScadenta"][0]).ToString("dd-MM-yyyy")));
                        cmd.Parameters.Add(new OracleParameter("valoare", Convert.ToDecimal(Request.Form["Valoare"][0], CultureInfo.InvariantCulture)));
                        cmd.Parameters.Add(new OracleParameter("id", id));

                        cmd.ExecuteNonQuery();

                        return RedirectToPage("/Facturi/Index");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                        return RedirectToPage("/Facturi/Index");
                    }
                }
            }
        }
    }
}