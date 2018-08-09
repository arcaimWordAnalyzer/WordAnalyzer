using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordAnalyzer.Core.Domain;
using WordAnalyzer.Core.Repositories;
using WordAnalyzer.Infrastructure.Commands;
using WordAnalyzer.Infrastructure.Converters;
using WordAnalyzer.Infrastructure.Sorts;

namespace WordAnalyzer.Infrastructure.Services
{
    public class SentenceService : ISentenceService
    {
        ISentenceRepository _sentenceRepository;
        ISentenceCreator _sentenceCreator;

        public SentenceService(ISentenceRepository sentenceRepository, ISentenceCreator sentenceCreator)
        {
            _sentenceRepository = sentenceRepository;
            _sentenceCreator = sentenceCreator;
        }

        public async Task<string> ConvertAsync(Converter converter)
        {
            await converter.LoadSentencesAsync(await _sentenceRepository.GetAllAsync());
            return await converter.ConvertToStructureAsync();
        }

        public async Task<bool> LoadAsync(string text)
        {
            await _sentenceCreator.CreateAsync(text);
            var sentences = await _sentenceCreator.GetAsync();
            await _sentenceRepository.ClearAsync();

            sentences.ToList()
                     .ForEach(x => _sentenceRepository
                     .AddAsync(x));
            
            return await _sentenceRepository.AnyAsync();
        }

        public async Task SortAsync(ISort sort)
        {
            if (sort != null)
                await sort.RunAsync(await _sentenceRepository.GetAllAsync());
        }
    }
}