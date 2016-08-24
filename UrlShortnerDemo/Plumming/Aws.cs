using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime;

namespace UrlShortnerDemo.Plumming
{
    public class Aws
    {
        public static AWSCredentials DefaultCedentials()
        {
            return new BasicAWSCredentials("AKIAIH2SK2QD5KQUO5YQ", "ASm4gcbTZCbOyb0OrChCPAPsxd+AQK17re8d2Pxk");
        }
    }
}
