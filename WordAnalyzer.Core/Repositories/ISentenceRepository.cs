using System.Collections.Generic;
using System.Threading.Tasks;
using WordAnalyzer.Core.Domain;

namespace WordAnalyzer.Core.Repositories
{
    public interface ISentenceRepository
    {
        Task AddAsync(Sentence sentence);
        Task<bool> AnyAsync();
        Task ClearAsync();
        Task<IEnumerable<Sentence>> GetAllAsync();
    }
}