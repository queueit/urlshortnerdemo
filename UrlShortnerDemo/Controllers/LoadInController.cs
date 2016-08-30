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
using Microsoft.Extensions.Primitives;
using UrlShortnerDemo.Models;

namespace UrlShortnerDemo.Controllers
{
    public class LoadInController : Controller
    {

        public async Task<ActionResult> Index(string key)
        {
            var model = await LoadModel(key);

            if (model == null)
                return Redirect("/");

            await LogRedirect(key);

            return Redirect(model.Url);
        }

        private async Task<UrlModel> LoadModel(string key)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(
                    "https://fai9q0sn2g.execute-api.eu-west-1.amazonaws.com/test/url?UrlKey=" + key);

                var responseString = await response.Content.ReadAsStringAsync();


                return new UrlModel() { UrlKey = key, Url = responseString.Trim('"')};
            }
        }

        private async Task LogRedirect(string key)
        {
            string userAgent = this.GetUserAgent();
            string ipAddress = this.GetIpAddress();

            var visitId = Guid.NewGuid().ToString();

            using (HttpClient client = new HttpClient())
            {
                var json = "{\"TableName\": \"UrlVisits\",\"Item\": {" +
                           "\"VisitId\": {\"S\": \"" + visitId + "\"}," +
                           "\"Visited\": {\"S\": \"" + DateTime.Now.ToString("O") + "\" }," +
                           "\"UrlKey\": {\"S\": \"" + key + "\"}," +
                           "\"User\": { \"M\": {" +
                           "\"IpAddress\":  {\"S\": \"" + ipAddress + "\"}," +
                           "\"UserAgent\": {\"S\": \"" + userAgent + "\"}}}}}";
                var response = await client.PutAsync(
                    "https://fai9q0sn2g.execute-api.eu-west-1.amazonaws.com/test/url",
                    new StringContent(json));
            }
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
