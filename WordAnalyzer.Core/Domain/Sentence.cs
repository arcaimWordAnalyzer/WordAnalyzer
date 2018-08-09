using System.Collections.Generic;

namespace WordAnalyzer.Core.Domain
{
    public class Sentence
    {
        public IEnumerable<string> Words { get; set; }

        public Sentence(IEnumerable<string> words)
        {
            Words = words;
        }
    }
}