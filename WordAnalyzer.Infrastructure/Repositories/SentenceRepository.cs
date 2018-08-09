using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordAnalyzer.Core.Domain;
using WordAnalyzer.Core.Repositories;

namespace WordAnalyzer.Infrastructure.Repositories
{
    public class SentenceRepository : ISentenceRepository
    {
        private ISet<Sentence> _sentences = new HashSet<Sentence>();

        public async Task AddAsync(Sentence sentence)
        {
            _sentences.Add(sentence);
            await Task.CompletedTask;
        }

        public async Task<bool> AnyAsync()
            => await Task.FromResult(_sentences.Any());

        public async Task ClearAsync()
        {
            _sentences.Clear();
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Sentence>> GetAllAsync()
            => await Task.FromResult(_sentences);
    }
}