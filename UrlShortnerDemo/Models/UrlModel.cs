using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;

namespace UrlShortnerDemo.Models
{
    [DynamoDBTable("Urls")]
    public class UrlModel
    {
        [DynamoDBHashKey]
        public string UrlKey { get; set; }
        [DynamoDBProperty]
        public string Url { get; set; }
        [DynamoDBProperty]
        public DateTime Created { get; set; }
        [DynamoDBProperty]
        public string User { get; set; }
        [DynamoDBProperty]
        public string ShortenedUrl { get; set; }
    }

    [DynamoDBTable("UrlVisits")]
    public class UrlVisitModel
    {
        [DynamoDBHashKey]
        public Guid VisitId { get; set; }
        [DynamoDBProperty]
        public string UrlKey { get; set; }
        [DynamoDBProperty]
        public DateTime Visited { get; set; }
        [DynamoDBProperty]
        public UserVisitModel User { get; set; }

    }

    public class UserVisitModel
    {
        [DynamoDBProperty]
        public string UserAgent { get; set; }
        [DynamoDBProperty]
        public string IpAddress { get; set; }
    }
}
