using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oracle.ManagedDataAccess.Client;
using ProiectBD.Data;
using ProiectBD.Enums;
using Index = ProiectBD.Data.Index;

namespace ProiectBD.Pages.Indecsi
{
    public class ModificaModel : PageModel
    {
        private readonly ILogger<ModificaModel> _logger;
        private readonly IConfiguration _configuration;

        public Index? Index { get; set; }

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
                        cmd.CommandText = "SELECT * FROM INDECSI WHERE ID = :id AND ROWNUM = 1";
                        cmd.Parameters.Add(new OracleParameter("id", id));

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            Index = new Index()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal(Index.Columns.Id)),
                                IdLocDeConsum = reader.GetInt32(reader.GetOrdinal(Index.Columns.IdLocDeConsum)),
                                TipIndex = Enum.Parse<TipIndex>(reader.GetString(reader.GetOrdinal(Index.Columns.TipIndex))),
                                Data = reader.GetDateTime(reader.GetOrdinal(Index.Columns.Data)),
                                Valoare = reader.GetInt32(reader.GetOrdinal(Index.Columns.Valoare))
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
                        cmd.CommandText = "UPDATE INDECSI SET ID_LOC_DE_CONSUM = :idLocDeConsum, TIP_INDEX = :tipIndex, DATA = :data, VALOARE = :valoare WHERE ID = :id";
                        cmd.Parameters.Add(new OracleParameter("idLocDeConsum", Request.Form["IdLocDeConsum"][0]));
                        cmd.Parameters.Add(new OracleParameter("tipIndex", Request.Form["TipIndex"][0]));
                        cmd.Parameters.Add(new OracleParameter("data", Convert.ToDateTime(Request.Form["Data"][0]).ToString("dd-MM-yyyy")));
                        cmd.Parameters.Add(new OracleParameter("valoare", Request.Form["Valoare"][0]));
                        cmd.Parameters.Add(new OracleParameter("id", id));

                        cmd.ExecuteNonQuery();

                        return RedirectToPage("/Indecsi/Index");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                        return RedirectToPage("/Indecsi/Index");
                    }
                }
            }
        }
    }
}