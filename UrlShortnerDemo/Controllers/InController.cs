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
using Microsoft.Extensions.Primitives;
using UrlShortnerDemo.Models;

namespace UrlShortnerDemo.Controllers
{
    public class InController : Controller
    {

        public ActionResult Index(string key)
        {
            using (var client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1))
            {
                using (DynamoDBContext context = new DynamoDBContext(client))
                {

                    var model = LoadModel(key, context);

                    if (model == null)
                        return Redirect("/");

                    LogRedirect(key, context);

                    return Redirect(model.Url);
                }
            }
        }

        private UrlModel LoadModel(string key, DynamoDBContext context)
        {
            return GetResult(context.LoadAsync<UrlModel>(key));
        }

        private void LogRedirect(string key, DynamoDBContext context)
        {
            string userAgent = this.GetUserAgent();
            string ipAddress = this.GetIpAddress();

            GetResult(context.SaveAsync(new UrlVisitModel()
            {
                VisitId = Guid.NewGuid(),
                UrlKey = key,
                Visited = DateTime.Now,
                User = new UserVisitModel()
                {
                    IpAddress = ipAddress,
                    UserAgent = userAgent
                }
            }));
        }

        public static void GetResult(Task task)
        {
            task.ConfigureAwait(false);
            task.Wait();
        }

        public static T GetResult<T>(Task<T> task)
        {
            task.ConfigureAwait(false);
            task.Wait();

            return task.Result;
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
