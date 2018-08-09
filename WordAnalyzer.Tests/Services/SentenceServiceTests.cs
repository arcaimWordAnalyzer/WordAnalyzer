using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using WordAnalyzer.Core.Domain;
using WordAnalyzer.Core.Repositories;
using WordAnalyzer.Infrastructure.Converters;
using WordAnalyzer.Infrastructure.Repositories;
using WordAnalyzer.Infrastructure.Services;
using WordAnalyzer.Infrastructure.Sorts;
using Xunit;

namespace WordAnalyzer.Tests.Services
{
    public class SentenceServiceTests
    {
        private string _sentence;
        private string _sentences;

        public SentenceServiceTests()
        {
            _sentence = "Mary had a little lamb.";
            _sentences = "  Mary   had a little  lamb  .\n\n\n"
                       + "  Peter   called for the wolf   ,  and Aesop came .\n"
                       + " Cinderella  likes shoes.";
        }

        [Fact]
        public async Task load_async_method_should_invoke_create_async_method_of_sentence_creator_class()
        {
            var sentenceCreatorMock = new Mock<ISentenceCreator>();
            var sentenceService = await GetSentenceSeviceAsync(null, sentenceCreatorMock);
            await sentenceService.LoadAsync(_sentence);

            sentenceCreatorMock.Verify(x => x.CreateAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task load_async_method_should_invoke_get_async_method_of_sentence_creator_class()
        {
            var sentenceCreatorMock = new Mock<ISentenceCreator>();
            var sentenceService = await GetSentenceSeviceAsync(null, sentenceCreatorMock);

            await sentenceService.LoadAsync(_sentence);

            sentenceCreatorMock.Verify(x => x.GetAsync(), Times.Once);
        }

        [Fact]
        public async Task load_async_sentence_should_invoke_clear_async_method_of_sentence_repository_class()
        {
            var sentenceRepositoryMock = new Mock<ISentenceRepository>();
            var sentenceService = await GetSentenceSeviceAsync(sentenceRepositoryMock);

            await sentenceService.LoadAsync(_sentence);
            
            sentenceRepositoryMock.Verify(x => x.ClearAsync(), Times.Once);
        }

        [Fact]
        public async Task load_async_method_should_invoke_add_async_method_of_sentence_repository_class()
        {
            var sentenceRepositoryMock = new Mock<ISentenceRepository>();
            var sentenceCreator = new SentenceCreator();

            var sentenceService = new SentenceService(sentenceRepositoryMock.Object, sentenceCreator);
            await sentenceService.LoadAsync(_sentence);

            sentenceRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Sentence>()), Times.Once);
        }

        [Fact]
        public async Task load_async_method_should_invoke_any_async_method_of_sentence_repository_class()
        {
            var sentenceRepositoryMock = new Mock<ISentenceRepository>();
            var sentenceService = await GetSentenceSeviceAsync(sentenceRepositoryMock);

            await sentenceService.LoadAsync(_sentence);
            
            sentenceRepositoryMock.Verify(x => x.AnyAsync(), Times.Once);
        }

        [Fact]
        public async Task load_async_method_should_add_async_sentence_to_sentence_repository()
        {
            Sentence expected = new Sentence(new [] { "Mary", "had", "a", "little", "lamb" });

            var actual = await GetSentenceAsync(_sentence);

            Assert.Equal(actual.Words, expected.Words);
        }

        [Fact]
        public async Task load_async_method_should_add_async_sentence_to_sentence_repository_with_invalid_charactes()
        {
            Sentence expected = new Sentence(new [] { "Ma&ry", "ha<d", ">a", "lit\"tle", "l'amb" });

            var actual = await GetSentenceAsync("Ma&ry ha<d >a lit\"tle l'amb");

            Assert.Equal(actual.Words, expected.Words);
        }

        private async Task<Sentence> GetSentenceAsync(string sentence)
        {
            var sentenceRepository = new SentenceRepository();
            var sentenceCreator = new SentenceCreator();
            var sentenceService = new SentenceService(sentenceRepository, sentenceCreator);

            bool isLoaded = await sentenceService.LoadAsync(sentence);

            return (await sentenceRepository.GetAllAsync()).FirstOrDefault();
        }

        [Fact]
        public async Task convert_and_sort_sentence_to_xml()
        {
            var expected = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>\r\n"
                            + "<text>\r\n"
                            + "  <sentence>\r\n"
                            + "    <word>a</word>\r\n"
                            + "    <word>had</word>\r\n"
                            + "    <word>lamb</word>\r\n"
                            + "    <word>little</word>\r\n"
                            + "    <word>Mary</word>\r\n"
                            + "  </sentence>\r\n"
                            + "</text>";

            var actual = await ConvertBase(_sentence, new XmlConverter());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task convert_and_sort_sentence_to_xml_with_invalid_charactes()
        {
            var expected = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>\r\n"
                            + "<text>\r\n"
                            + "  <sentence>\r\n"
                            + "    <word>a</word>\r\n"
                            + "    <word>had</word>\r\n"
                            + "    <word>lamb</word>\r\n"
                            + "    <word>lit&amp;gt;tle</word>\r\n"
                            + "    <word>Ma&amp;lt;ry</word>\r\n"
                            + "  </sentence>\r\n"
                            + "  <sentence>\r\n"
                            + "    <word>&amp;amp;</word>\r\n"
                            + "    <word>&amp;apos;Aesop</word>\r\n"
                            + "    <word>and</word>\r\n"
                            + "    <word>call&amp;quot;ed</word>\r\n"
                            + "    <word>came</word>\r\n"
                            + "    <word>for</word>\r\n"
                            + "    <word>P&amp;amp;eter</word>\r\n"
                            + "    <word>the</word>\r\n"
                            + "    <word>wolf</word>\r\n"
                            + "  </sentence>\r\n"
                            + "</text>";

            var sentence = "  Ma<ry   had a lit>tle  lamb  .\n\n\n&"
                         + "  P&eter   call\"ed for the wolf   ,  and \'Aesop came .\n";

 	        var actual = await ConvertBase(sentence, new XmlConverter());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task convert_sentence_to_csv()
        {
            string expected = ", Word 1, Word 2, Word 3, Word 4, Word 5\r\n"
                            + "Sentence 1, a, had, lamb, little, Mary";

            var actual = await ConvertBase(_sentence, new CsvConverter());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task convert_sentence_to_csv_with_invalid_characters_for_xml()
        {
            string expected = ", Word 1, Word 2, Word 3, Word 4, Word 5\r\n"
                            + "Sentence 1, >a, ha<d, l'amb, lit\"tle, Ma&ry";

            var actual = await ConvertBase("Ma&ry ha<d >a lit\"tle l'amb", new CsvConverter());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task convert_and_sort_sentences_to_xml()
        {
            string expected = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>\r\n"
                            + "<text>\r\n"
                            + "  <sentence>\r\n"
                            + "    <word>a</word>\r\n"
                            + "    <word>had</word>\r\n"
                            + "    <word>lamb</word>\r\n"
                            + "    <word>little</word>\r\n"
                            + "    <word>Mary</word>\r\n"
                            + "  </sentence>\r\n"
                            + "  <sentence>\r\n"
                            + "    <word>Aesop</word>\r\n"
                            + "    <word>and</word>\r\n"
                            + "    <word>called</word>\r\n"
                            + "    <word>came</word>\r\n"
                            + "    <word>for</word>\r\n"
                            + "    <word>Peter</word>\r\n"
                            + "    <word>the</word>\r\n"
                            + "    <word>wolf</word>\r\n"
                            + "  </sentence>\r\n"
                            + "  <sentence>\r\n"
                            + "    <word>Cinderella</word>\r\n"
                            + "    <word>likes</word>\r\n"
                            + "    <word>shoes</word>\r\n"
                            + "  </sentence>\r\n"
                            + "</text>";

            var actual = await ConvertBase(_sentences, new XmlConverter());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task convert_sentences_to_csv()
        {
            string expected = ", Word 1, Word 2, Word 3, Word 4, Word 5, Word 6, Word 7, Word 8\r\n"
                            + "Sentence 1, a, had, lamb, little, Mary\r\n"
                            + "Sentence 2, Aesop, and, called, came, for, Peter, the, wolf\r\n"
                            + "Sentence 3, Cinderella, likes, shoes";

            var actual = await ConvertBase(_sentences, new CsvConverter());

            Assert.Equal(expected, actual);
        }

        private async Task<SentenceService> GetSentenceSeviceAsync(Mock<ISentenceRepository> sentenceRepositoryMock = null,
                                                                       Mock<ISentenceCreator> sentenceCreatorMock = null)
        {
            sentenceRepositoryMock = sentenceRepositoryMock ?? new Mock<ISentenceRepository>();
            sentenceCreatorMock = sentenceCreatorMock ?? new Mock<ISentenceCreator>();

            return await Task.FromResult(new SentenceService(sentenceRepositoryMock.Object, sentenceCreatorMock.Object));
        }

        private async Task<string> ConvertBase(string text, Converter converter)
        {
            var sentenceRepository = new SentenceRepository();
            var sentenceCreator = new SentenceCreator();
            var sentenceService = new SentenceService(sentenceRepository, sentenceCreator);

            var isLoaded = await sentenceService.LoadAsync(text);
            await sentenceService.SortAsync(new SortAsc());
            return isLoaded ? await sentenceService.ConvertAsync(converter) : "Error!";
        }
    }
}