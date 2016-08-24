using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Microsoft.AspNetCore.Mvc;
using UrlShortnerDemo.Models;
using UrlShortnerDemo.Plumming;

namespace UrlShortnerDemo.Controllers
{
    public class InController : Controller
    {
        
        public async Task<ActionResult> Index(string key)
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1);
            DynamoDBContext context = new DynamoDBContext(client);

            var model = await LoadModel(key, context);

            if (model == null)
                return Redirect("/");

            await LogRedirect(key, client);

            return Redirect(model.Url);
        }

        private async Task<UrlModel> LoadModel(string key, DynamoDBContext context)
        {
            UrlModel model = await context.LoadAsync<UrlModel>(key);
            return model;
        }

        private async Task LogRedirect(string key, AmazonDynamoDBClient client)
        {
            string userMetadata = this.Request.Headers["User-Agent"];

            await client.UpdateItemAsync(new UpdateItemRequest(
                "Urls",
                new Dictionary<string, AttributeValue>() { { "Key", new AttributeValue() { S = key } } },
                new Dictionary<string, AttributeValueUpdate>()
                {
                    {
                        "Redirected", new AttributeValueUpdate(new AttributeValue()
                        {
                            L = new List<AttributeValue>()
                            {
                                new AttributeValue
                                {
                                    M = new Dictionary<string, AttributeValue>()
                                    {
                                        {"RedirectTime", new AttributeValue() {S = DateTime.UtcNow.ToString("O")}},
                                        {"UserAgent", new AttributeValue() {S = userMetadata}}
                                    }
                                }
                            }
                        },
                        AttributeAction.ADD)
                    }
                })
            );
        }
    }
}
