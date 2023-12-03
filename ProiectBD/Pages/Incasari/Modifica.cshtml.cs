using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oracle.ManagedDataAccess.Client;
using ProiectBD.Data;
using ProiectBD.Enums;
using System.Globalization;

namespace ProiectBD.Pages.Incasari
{
    public class ModificaModel : PageModel
    {
        private readonly ILogger<ModificaModel> _logger;
        private readonly IConfiguration _configuration;

        public Incasare? Incasare { get; set; }

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
                        cmd.CommandText = "SELECT * FROM INCASARI WHERE ID = :id AND ROWNUM = 1";
                        cmd.Parameters.Add(new OracleParameter("id", id));

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            Incasare = new Incasare()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal(Incasare.Columns.Id)),
                                IdFactura = reader.GetInt32(reader.GetOrdinal(Incasare.Columns.IdFactura)),
                                Data = reader.GetDateTime(reader.GetOrdinal(Incasare.Columns.Data)),
                                Valoare = reader.GetDecimal(reader.GetOrdinal(Incasare.Columns.Valoare))
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
                        cmd.CommandText = "UPDATE INCASARI SET ID_FACTURA = :idFactura, DATA = :data, VALOARE = :valoare WHERE ID = :id";
                        cmd.Parameters.Add(new OracleParameter("idFactura", Request.Form["IdFactura"][0]));
                        cmd.Parameters.Add(new OracleParameter("data", Convert.ToDateTime(Request.Form["Data"][0]).ToString("dd-MM-yyyy")));
                        cmd.Parameters.Add(new OracleParameter("valoare", Convert.ToDecimal(Request.Form["Valoare"][0], CultureInfo.InvariantCulture)));
                        cmd.Parameters.Add(new OracleParameter("id", id));

                        cmd.ExecuteNonQuery();

                        return RedirectToPage("/Incasari/Index");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                        return RedirectToPage("/Incasari/Index");
                    }
                }
            }
        }
    }
}