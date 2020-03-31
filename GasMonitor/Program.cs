using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace GasMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            var task = AmazonService.ReadObjectDataAsync();
            task.Wait();
            Console.WriteLine("data {0}", task.Result);
    
        }
    }
}    