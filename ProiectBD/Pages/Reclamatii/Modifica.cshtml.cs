using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oracle.ManagedDataAccess.Client;
using ProiectBD.Data;
using ProiectBD.Enums;

namespace ProiectBD.Pages.Reclamatii
{
    public class ModificaModel : PageModel
    {
        private readonly ILogger<ModificaModel> _logger;
        private readonly IConfiguration _configuration;

        public Reclamatie? Reclamatie { get; set; }

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
                        cmd.CommandText = "SELECT * FROM RECLAMATII WHERE ID = :id AND ROWNUM = 1";
                        cmd.Parameters.Add(new OracleParameter("id", id));

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            Reclamatie = new Reclamatie()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal(Reclamatie.Columns.Id)),
                                IdClient = reader.GetInt32(reader.GetOrdinal(Reclamatie.Columns.IdClient)),
                                Informatii = reader.GetString(reader.GetOrdinal(Reclamatie.Columns.Informatii)),
                                DataInregistrare = reader.GetDateTime(reader.GetOrdinal(Reclamatie.Columns.DataInregistrare)),
                                DataSolutionare = reader.IsDBNull(reader.GetOrdinal(Reclamatie.Columns.DataSolutionare)) ? null : reader.GetDateTime(reader.GetOrdinal(Reclamatie.Columns.DataSolutionare))
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
                        cmd.CommandText = "UPDATE RECLAMATII SET ID_CLIENT = :idClient, INFORMATII = :informatii, DATA_INREGISTRARE = :dataInregistrare, DATA_SOLUTIONARE = :dataSolutionare WHERE ID = :id";
                        cmd.Parameters.Add(new OracleParameter("idClient", Request.Form["IdClient"][0]));
                        cmd.Parameters.Add(new OracleParameter("informatii", Request.Form["Informatii"][0]));
                        cmd.Parameters.Add(new OracleParameter("dataInregistrare", Convert.ToDateTime(Request.Form["DataInregistrare"][0]).ToString("dd-MM-yyyy")));
                        cmd.Parameters.Add(new OracleParameter("dataSolutionare", string.IsNullOrWhiteSpace(Request.Form["DataSolutionare"].ToString()) ? null : Convert.ToDateTime(Request.Form["DataSolutionare"][0]).ToString("dd-MM-yyyy")));
                        cmd.Parameters.Add(new OracleParameter("id", id));

                        cmd.ExecuteNonQuery();

                        return RedirectToPage("/Reclamatii/Index");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                        return RedirectToPage("/Reclamatii/Index");
                    }
                }
            }
        }
    }
}