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
        public ActionResult Index(UrlModel model)
        {
            model.UrlKey = GenerateKey();
            model.Created = DateTime.UtcNow;
            model.User = "testuser";
            model.ShortenedUrl = GenerateShortUrl(model.UrlKey);

            AddToDynamo(model);

            return View(model);
        }

        public ActionResult Error()
        {
            return View();
        }

        public static void GetResult(Task task)
        {
            task.ConfigureAwait(false);
            task.Wait();
        }

        private void AddToDynamo(UrlModel model)
        {
            
            using (var client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1))
            {
                using (DynamoDBContext context = new DynamoDBContext(client))
                {
                    GetResult(context.SaveAsync(model));
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
