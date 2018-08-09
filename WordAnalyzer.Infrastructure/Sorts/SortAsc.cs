using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordAnalyzer.Core.Domain;

namespace WordAnalyzer.Infrastructure.Sorts
{
    public class SortAsc : ISort
    {
        public async Task RunAsync(IEnumerable<Sentence> sentences)
        {
            foreach (var sen in sentences)
                sen.Words = sen.Words.OrderBy(x => x);
            
            await Task.CompletedTask;
        }
    }
}