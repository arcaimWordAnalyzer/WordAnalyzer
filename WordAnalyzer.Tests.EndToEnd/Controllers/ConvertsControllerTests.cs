using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using WordAnalyzer.Api;
using WordAnalyzer.Infrastructure.Commands;
using Xunit;

namespace WordAnalyzer.Tests.EndToEnd.Controllers
{
    public class ConvertsControllerTests
    {
        private string _sentence;
        private TestServer _server;
        private HttpClient _client;

        private readonly LoadText _request = new LoadText
        {
            Text = "  Mary   had a little  lamb  .\n\n\n"
                    + "  Peter   called for the wolf   ,  and Aesop came .\n"
                    + " Cinderella  likes shoes."
        };

        public ConvertsControllerTests()
        {
            var webHostBuilder = new WebHostBuilder().UseStartup<Startup>();
            _server = new TestServer(webHostBuilder);
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task convert_sentences_to_xml()
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
            var action = "ConvertToXml";
            var actual = await InvokeAsync(action, _request);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task convert_sentences_to_csv()
        {
            string expected = ", Word 1, Word 2, Word 3, Word 4, Word 5, Word 6, Word 7, Word 8\r\n"
                            + "Sentence 1, a, had, lamb, little, Mary\r\n"
                            + "Sentence 2, Aesop, and, called, came, for, Peter, the, wolf\r\n"
                            + "Sentence 3, Cinderella, likes, shoes";
            var action = "ConvertToCsv";
            var actual = await InvokeAsync(action, _request);

            actual.Should().BeEquivalentTo(expected);
        }

        public async Task<string> InvokeAsync(string action, object data)
        {
            var payload = GetPayload(data);
            var postResponse = await _client.PostAsync("/Converts/load", payload);
            postResponse.EnsureSuccessStatusCode();

            var getResponse = await _client.GetAsync($"/Converts/{action}");
            getResponse.EnsureSuccessStatusCode();

            return await getResponse.Content.ReadAsStringAsync();
        }

        private static StringContent GetPayload(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}