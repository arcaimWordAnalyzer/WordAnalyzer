using System.Collections.Generic;
using System.Threading.Tasks;
using WordAnalyzer.Core.Domain;
using WordAnalyzer.Infrastructure.Commands;
using WordAnalyzer.Infrastructure.Converters;
using WordAnalyzer.Infrastructure.Sorts;

namespace WordAnalyzer.Infrastructure.Services
{
    public interface ISentenceService
    {
        Task<string> ConvertAsync(Converter converter);
        Task<bool> LoadAsync(string text);
        
        Task SortAsync(ISort sort);
    }
}