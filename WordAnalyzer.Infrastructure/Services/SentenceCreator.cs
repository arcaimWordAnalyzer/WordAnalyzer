using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordAnalyzer.Core.Domain;

namespace WordAnalyzer.Infrastructure.Services
{
    public class SentenceCreator : ISentenceCreator
    {
        public List<Sentence> _sentences { get; }

        public SentenceCreator()
        {
            _sentences = new List<Sentence>();
        }

        public async Task CreateAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;
            
            text.Replace(",","")
                .SplitBy('.')
                .ToList()
                .ForEach(x => _sentences
                .Add(CreateSentence(x)));

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Sentence>> GetAsync()
            => await Task.FromResult(_sentences);

        private Sentence CreateSentence(string rawSentence)
            => new Sentence(rawSentence.Replace("\n", " ")
                          .Replace("\r", " ")
                          .Replace("\t", " ")
                          .Trim()
                          .SplitBy(' '));
    }

    internal static class ExtensionMethods
    {
        public static IEnumerable<string> SplitBy(this string text, char c)
            => text.Split(c)
                   .Where(x => !string.IsNullOrWhiteSpace(x));
    }
}