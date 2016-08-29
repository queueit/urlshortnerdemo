using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Microsoft.AspNetCore.Mvc;
using UrlShortnerDemo.Models;
using UrlShortnerDemo.Plumming;

namespace UrlShortnerDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(UrlModel model)
        {
            model.UrlKey = GenerateKey();
            model.Created = DateTime.UtcNow;
            model.User = "testuser";
            model.ShortenedUrl = GenerateShortUrl(model.UrlKey);

            await AddToDynamo(model);

            return View(model);
        }

        private async Task AddToDynamo(UrlModel model)
        {

            AmazonDynamoDBClient client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1);
            DynamoDBContext context = new DynamoDBContext(client);

            await context.SaveAsync(model);

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
