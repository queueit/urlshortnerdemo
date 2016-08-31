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
    public class HomeController : Controller
    {
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
            using (var client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1))
            {
                using (DynamoDBContext context = new DynamoDBContext(client))
                {
                    await context.SaveAsync(model).ConfigureAwait(false);
                }
            }
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
