using System.Collections.Generic;
using System.Threading.Tasks;
using WordAnalyzer.Core.Domain;
using WordAnalyzer.Core.Repositories;
using WordAnalyzer.Infrastructure.Sorts;

namespace WordAnalyzer.Infrastructure.Converters
{
    public abstract class Converter
    {
        protected IEnumerable<Sentence> _sentences;
        public async Task LoadSentencesAsync(IEnumerable<Sentence> sentences)
        {
            _sentences = sentences;
            await Task.CompletedTask;
        }

        public async Task<string> ConvertToStructureAsync()
            => await CreateStructureAsync(_sentences);

        protected abstract Task<string> CreateStructureAsync(IEnumerable<Sentence> senteces);
    }
}