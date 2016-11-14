using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Microsoft.AspNetCore.Mvc;
using UrlShortnerDemo.Models;

namespace UrlShortnerDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAmazonDynamoDB _client;

        public HomeController(IAmazonDynamoDB client)
        {
            _client = client;
            //_client = new AmazonDynamoDBClient(
            //    new BasicAWSCredentials("AKIAI4JMFBLBIUI5Y35A", "e/DNfxfU/TzzZQCCl4wJctPFh+HXGo/OuIa9gCxm"),
            //    RegionEndpoint.EUWest1);
            //_context = new DynamoDBContext(_client);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult> Index(UrlModel model)
        {
            model.UrlKey = GenerateKey();
            model.Created = DateTime.UtcNow;
            model.User = "testuser";
            model.ShortenedUrl = GenerateShortUrl(model.UrlKey);

            await AddToDynamo(model).ConfigureAwait(false);

            return View(model);
        }

        private async Task AddToDynamo(UrlModel model)
        {
            await ConcurrencyHelper.ExecuteAsync(async () => await _client.PutItemAsync("Urls", new Dictionary<string, AttributeValue>()
                {
                    {"UrlKey", new AttributeValue() {S = model.UrlKey}},
                    {"Created", new AttributeValue() {S = model.Created.ToString("O")}},
                    {"User", new AttributeValue() {S = model.User}},
                    {"ShortenedUrl", new AttributeValue() {S = model.ShortenedUrl}},
                    {"Url", new AttributeValue() {S = model.Url}}
                }).ConfigureAwait(false));

            //await _context.SaveAsync(model).ConfigureAwait(false);
        }

        public ActionResult Error()
        {
            return View();
        }

        private string GenerateShortUrl(string key)
        {
            return "http://url.realvaluetalks.com/in/" + key;
        }

        private string GenerateKey()
        {
            return Guid.NewGuid().ToString().Substring(0, 8);
        }
    }
}
