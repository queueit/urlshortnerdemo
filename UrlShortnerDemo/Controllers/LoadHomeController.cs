using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    public class LoadHomeController : Controller
    {
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult> Index(UrlModel model)
        {
            model.UrlKey = GenerateKey();
            model.Created = DateTime.UtcNow;
            model.User = "testuser";
            model.ShortenedUrl = GenerateShortUrl(model.UrlKey);

            using (HttpClient client = new HttpClient())
            {
                var json = "{\"TableName\": \"Urls\",\"Item\": {" +
                           "\"UrlKey\": {\"S\": \"" + model.UrlKey + "\"}," +
                           "\"Created\": {\"S\": \"" + model.Created.ToString("O") + "\" }," +
                           "\"ShortenedUrl\": {\"S\": \"" + model.ShortenedUrl + "\"}," +
                           "\"Url\": {\"S\": \"" + model.Url + "\"}," +
                           "\"User\": {\"S\": \"" + model.User + "\"}}}";
                var response = await client.PutAsync(
                    "https://fai9q0sn2g.execute-api.eu-west-1.amazonaws.com/test/url",
                    new StringContent(json));
            }
            return View(model);
        }

        public ActionResult Error()
        {
            return View();
        }


        private async Task AddToDynamo(UrlModel model)
        {
            using (var client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1))
            {
                using (DynamoDBContext Context = new DynamoDBContext(client))
                {
                    await Context.SaveAsync(model).ConfigureAwait(false);
                }
            }
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
