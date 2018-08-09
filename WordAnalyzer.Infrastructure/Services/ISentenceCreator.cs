using System.Collections.Generic;
using System.Threading.Tasks;
using WordAnalyzer.Core.Domain;

namespace WordAnalyzer.Infrastructure.Services
{
    public interface ISentenceCreator
    {
        Task CreateAsync(string text);
        Task<IEnumerable<Sentence>> GetAsync();
    }
}