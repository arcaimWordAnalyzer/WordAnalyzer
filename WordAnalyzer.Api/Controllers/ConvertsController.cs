using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WordAnalyzer.Infrastructure.Commands;
using WordAnalyzer.Infrastructure.Converters;
using WordAnalyzer.Infrastructure.Services;
using WordAnalyzer.Infrastructure.Sorts;

namespace WordAnalyzer.Api.Controllers
{
    public class ConvertsController : Controller
    {
        readonly ISentenceService _sentenceService;

        public ConvertsController(ISentenceService sentenceService)
        {
            _sentenceService = sentenceService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost()]
        public async Task<IActionResult> Load([FromBody] LoadText command)
            => await _sentenceService.LoadAsync(command.Text)
               ? StatusCode(201)
               : StatusCode(204);

        public async Task<string> ConvertToXml()
        {
            await _sentenceService.SortAsync(new SortAsc());
            return await _sentenceService.ConvertAsync(new XmlConverter());
        }

        public async Task<string> ConvertToCsv()
        {
            await _sentenceService.SortAsync(new SortAsc());
            return await _sentenceService.ConvertAsync(new CsvConverter());
        }
    }
        
}