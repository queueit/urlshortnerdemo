using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using UrlShortnerDemo.Models;

namespace UrlShortnerDemo.Controllers
{
    public class InController : Controller
    {
        private readonly IAmazonDynamoDB _client;

        public InController(IAmazonDynamoDB client)
        {
            _client = client;
        }

        public async Task<ActionResult> Index(string key)
        {
            return await ConcurrencyHelper.ExecuteAsync<ActionResult>(async () =>
            {
                var model = await LoadModel(key).ConfigureAwait(false);

                if (model == null)
                    return Redirect("/");

                await LogRedirect(key).ConfigureAwait(false);

                return Redirect(model);
            });
        }

        private async Task<string> LoadModel(string key)
        {
            //using (var _client = new AmazonDynamoDBClient(
            //    new BasicAWSCredentials("AKIAI4JMFBLBIUI5Y35A", "e/DNfxfU/TzzZQCCl4wJctPFh+HXGo/OuIa9gCxm"),
            //    RegionEndpoint.EUWest1))
            //{

                var result = await _client.GetItemAsync("Urls", new Dictionary<string, AttributeValue>()
                {
                    {"UrlKey", new AttributeValue() {S = key}}
                });

                return result.Item["Url"].S;
                //return await context.LoadAsync<UrlModel>(key).ConfigureAwait(false);
            //}
        }

        private async Task LogRedirect(string key)
        {
            string userAgent = this.GetUserAgent();
            string ipAddress = this.GetIpAddress();
            //using (var _client = new AmazonDynamoDBClient(
            //    new BasicAWSCredentials("AKIAI4JMFBLBIUI5Y35A", "e/DNfxfU/TzzZQCCl4wJctPFh+HXGo/OuIa9gCxm"),
            //    RegionEndpoint.EUWest1))
            //{

                var response = await _client.PutItemAsync("UrlVisits", new Dictionary<string, AttributeValue>()
                {
                    {"VisitId", new AttributeValue() {S = Guid.NewGuid().ToString()}},
                    {"UrlKey", new AttributeValue() {S = key}},
                    {"Visited", new AttributeValue() {S = DateTime.UtcNow.ToString("O")}},
                    {
                        "User", new AttributeValue()
                        {
                            M = new Dictionary<string, AttributeValue>()
                            {
                                {"IpAddress", new AttributeValue() {S = ipAddress}},
                                {"UserAgent", new AttributeValue() {S = userAgent}}
                            }
                        }
                    }
                }).ConfigureAwait(false);
            //}
            //await context.SaveAsync(new UrlVisitModel()
            //{
            //    VisitId = Guid.NewGuid(),
            //    UrlKey = key,
            //    Visited = DateTime.Now,
            //    User = new UserVisitModel()
            //    {
            //        IpAddress = ipAddress,
            //        UserAgent = userAgent
            //    }
            //}).ConfigureAwait(false);
        }

        private StringValues GetUserAgent()
        {
            return this.Request.Headers["User-Agent"];
        }

        private string GetIpAddress()
        {
            if (this.Request.Headers.ContainsKey("X-ForwardedFor"))
                return this.Request.Headers["X-ForwardedFor"];

            return this.Request.Host.Host;
        }
    }
}
