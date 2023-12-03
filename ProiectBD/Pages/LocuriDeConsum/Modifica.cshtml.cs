using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oracle.ManagedDataAccess.Client;
using ProiectBD.Data;
using ProiectBD.Enums;

namespace ProiectBD.Pages.LocuriDeConsum
{
    public class ModificaModel : PageModel
    {
        private readonly ILogger<ModificaModel> _logger;
        private readonly IConfiguration _configuration;

        public LocDeConsum? LocDeConsum { get; set; }

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
                        cmd.CommandText = "SELECT * FROM LOCURI_DE_CONSUM WHERE ID = :id AND ROWNUM = 1";
                        cmd.Parameters.Add(new OracleParameter("id", id));

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            LocDeConsum = new LocDeConsum()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal(LocDeConsum.Columns.Id)),
                                IdClient = reader.GetInt32(reader.GetOrdinal(LocDeConsum.Columns.IdClient)),
                                Judet = reader.GetString(reader.GetOrdinal(LocDeConsum.Columns.Judet)),
                                Localitate = reader.GetString(reader.GetOrdinal(LocDeConsum.Columns.Localitate)),
                                Strada = reader.GetString(reader.GetOrdinal(LocDeConsum.Columns.Strada)),
                                NrStrada = reader.GetInt32(reader.GetOrdinal(LocDeConsum.Columns.NrStrada)),
                                CodPostal = reader.GetString(reader.GetOrdinal(LocDeConsum.Columns.CodPostal)),
                                Bloc = reader.IsDBNull(reader.GetOrdinal(LocDeConsum.Columns.Bloc)) ? null : reader.GetString(reader.GetOrdinal(LocDeConsum.Columns.Bloc)),
                                Scara = reader.IsDBNull(reader.GetOrdinal(LocDeConsum.Columns.Scara)) ? null : reader.GetString(reader.GetOrdinal(LocDeConsum.Columns.Scara)),
                                Etaj = reader.IsDBNull(reader.GetOrdinal(LocDeConsum.Columns.Etaj)) ? null : reader.GetInt32(reader.GetOrdinal(LocDeConsum.Columns.Etaj)),
                                Apartament = reader.IsDBNull(reader.GetOrdinal(LocDeConsum.Columns.Apartament)) ? null : reader.GetInt32(reader.GetOrdinal(LocDeConsum.Columns.Apartament))
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
                        cmd.CommandText = "UPDATE LOCURI_DE_CONSUM SET ID_CLIENT = :idClient, JUDET = :judet, LOCALITATE = :localitate, STRADA = :strada, NUMAR_STRADA = :nrStrada, COD_POSTAL = :codPostal, BLOC = :bloc, SCARA = :scara, ETAJ = :etaj, APARTAMENT = :apartament WHERE ID = :id";
                        cmd.Parameters.Add(new OracleParameter("idClient", Request.Form["IdClient"][0]));
                        cmd.Parameters.Add(new OracleParameter("judet", Request.Form["Judet"][0]));
                        cmd.Parameters.Add(new OracleParameter("localitate", Request.Form["Localitate"][0]));
                        cmd.Parameters.Add(new OracleParameter("strada", Request.Form["Strada"][0]));
                        cmd.Parameters.Add(new OracleParameter("nrStrada", Request.Form["NrStrada"][0]));
                        cmd.Parameters.Add(new OracleParameter("codPostal", Request.Form["CodPostal"][0]));
                        cmd.Parameters.Add(new OracleParameter("bloc", Request.Form["Bloc"][0]));
                        cmd.Parameters.Add(new OracleParameter("scara", Request.Form["Scara"][0]));
                        cmd.Parameters.Add(new OracleParameter("etaj", Request.Form["Etaj"][0]));
                        cmd.Parameters.Add(new OracleParameter("apartament", Request.Form["Apartament"][0]));
                        cmd.Parameters.Add(new OracleParameter("id", id));

                        cmd.ExecuteNonQuery();

                        return RedirectToPage("/LocuriDeConsum/Index");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                        return RedirectToPage("/LocuriDeConsum/Index");
                    }
                }
            }
        }
    }
}