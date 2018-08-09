using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordAnalyzer.Core.Domain;
using WordAnalyzer.Core.Repositories;
using WordAnalyzer.Infrastructure.Sorts;

namespace WordAnalyzer.Infrastructure.Converters
{
    public class CsvConverter : Converter
    {
        protected override async Task<string> CreateStructureAsync(IEnumerable<Sentence> sentences)
        {
            var max = sentences.Select(sen => new 
                                    { WordsNumber = sen.Words.Count() })
                                .Max(x => x.WordsNumber);

            var csv = new StringBuilder();

            for (int i = 1; i <= max; i++)
                csv.Append($", Word {i}");
            csv.AppendLine();

            int j = 1;
            
            foreach(var sen in sentences)
            {
                csv.AppendLine($"Sentence {j++}, " + string.Join(", ", sen.Words));
            }

            return await Task.FromResult(csv.ToString().TrimEnd());
        }
    }
}