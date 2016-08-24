using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;

namespace UrlShortnerDemo.Models
{
    [DynamoDBTable("Urls")]
    public class UrlModel
    {
        [DynamoDBHashKey]
        public string Key { get; set; }
        [DynamoDBProperty]
        public string Url { get; set; }
        [DynamoDBProperty]
        public string Created { get; set; }
        [DynamoDBProperty]
        public string User { get; set; }
        [DynamoDBProperty]
        public string ShortenedUrl { get; set; }
    }
}
