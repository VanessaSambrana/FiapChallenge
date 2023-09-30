using Microsoft.AspNetCore.Mvc;
using Procurados.Data;
using Procurados.Models;

namespace Procurados.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
	public class IdwallController : Controller
	{
        private readonly IdwallContext _context;

        public IdwallController(IdwallContext context)
		{
            _context = context ?? throw new ArgumentException(nameof(context));

            HttpClient httpClient = new HttpClient();
            _context.SetHttpClient(httpClient);
        }

        private List<IdwallUser> wanteds = new List<IdwallUser>();

        [HttpGet]
        public ActionResult<IEnumerable<IdwallUser>> Get()
        {
            var registros = _context.User.ToList();

            return Ok(registros);
        }

        [HttpGet("FBI")]
        public async Task<ActionResult<string>> FBIMostWanted(string title, IdwallUser wanted)
        {
            bool isWanted = await _context.ProcuradoFBIByTitle(title);

            if (isWanted)
            {
                wanteds.Add(wanted);
                return $"{title} é procurado pelo FBI";
            }
            else
            {
                return $"{title} não é procurado pelo FBI.";
            }
        }

        [HttpGet("Interpol")]
        public async Task<IActionResult> InterpolMostWanted(string forename, string name, string? dateOfBirth = null)
        {
            bool isWanted = await _context.ProcuradoInterpol(forename, name, dateOfBirth);


            if (isWanted && dateOfBirth == null)
            {
                return Ok($"{forename} {name}, é procurado pela Interpol.");
            }
            if (isWanted)
            {
                return Ok($"{forename} {name}, nascido em {dateOfBirth} é procurado pela Interpol.");

            }

            return Ok($"{forename} {name} não é procurado pela Interpol.");
        }

    }
}


  


       

       


