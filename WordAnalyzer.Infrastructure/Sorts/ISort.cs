using System.Collections.Generic;
using System.Threading.Tasks;
using WordAnalyzer.Core.Domain;

namespace WordAnalyzer.Infrastructure.Sorts
{
    public interface ISort
    {
        Task RunAsync(IEnumerable<Sentence> sentences);
    }
}