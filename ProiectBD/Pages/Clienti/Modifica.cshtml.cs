using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oracle.ManagedDataAccess.Client;
using ProiectBD.Data;
using ProiectBD.Enums;

namespace ProiectBD.Pages.Clienti
{
    public class ModificaModel : PageModel
    {
        private readonly ILogger<ModificaModel> _logger;
        private readonly IConfiguration _configuration;

        public Client? Client { get; set; }

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
                        cmd.CommandText = "SELECT * FROM CLIENTI WHERE ID = :id AND ROWNUM = 1";
                        cmd.Parameters.Add(new OracleParameter("id", id));

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            Client = new Client()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal(Client.Columns.Id)),
                                Nume = reader.GetString(reader.GetOrdinal(Client.Columns.Nume)),
                                Email = reader.GetString(reader.GetOrdinal(Client.Columns.Email)),
                                Telefon = reader.GetString(reader.GetOrdinal(Client.Columns.Telefon)),
                                TipPersoana = Enum.Parse<TipPersoana>(reader.GetString(reader.GetOrdinal(Client.Columns.TipPersoana)))
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
                        cmd.CommandText = "UPDATE CLIENTI SET NUME = :nume, EMAIL = :email, TELEFON = :telefon, TIP_PERSOANA = :tipPersoana WHERE ID = :id";
                        cmd.Parameters.Add(new OracleParameter("nume", Request.Form["Nume"][0]));
                        cmd.Parameters.Add(new OracleParameter("email", Request.Form["Email"][0]));
                        cmd.Parameters.Add(new OracleParameter("telefon", Request.Form["Telefon"][0]));
                        cmd.Parameters.Add(new OracleParameter("tipPersoana", Request.Form["TipPersoana"][0]));
                        cmd.Parameters.Add(new OracleParameter("id", id));

                        cmd.ExecuteNonQuery();

                        return RedirectToPage("/Clienti/Index");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                        return RedirectToPage("/Clienti/Index");
                    }
                }
            }
        }
    }
}