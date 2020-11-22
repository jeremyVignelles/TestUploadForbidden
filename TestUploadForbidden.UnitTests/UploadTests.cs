using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestUploadForbidden.UnitTests
{
    public class UploadTests
    {
        private IHost _host;

        [OneTimeSetUp]
        public async Task Setup()
        {
            this._host = Program.CreateHostBuilder(new string[] { "--urls", "https://localhost:5001", "--environment", "Development" }).Build();
            await this._host.StartAsync();
        }

        [OneTimeTearDown]
        public async Task Teardown()
        {
            await this._host.StopAsync();
            this._host.Dispose();
        }


        [Test]
        public async Task SmallFileUploadAsync()
        {
            await this.DoTestAsync(1_000_000);
        }

        [Test]
        public async Task BigFileUploadAsync()
        {
            await this.DoTestAsync(256_000_000);
        }

        private async Task DoTestAsync(int uploadSize)
        {
            using var client = new HttpClient();
            var buffer = new byte[uploadSize];
            await using var ms = new MemoryStream(buffer);
            using var fileContent = new StreamContent(ms);
            var formContent = new MultipartFormDataContent(Guid.NewGuid().ToString())
            {
                {fileContent, "file", "uploadedFile"}
            };
            using var message =
                new HttpRequestMessage(HttpMethod.Post, "https://localhost:5001/")
                {
                    Content = formContent
                };
            using var response = await client.SendAsync(message);
            var body = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}