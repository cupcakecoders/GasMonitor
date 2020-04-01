using System;
using System.Collections.Generic;
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
            var list = AmazonService.ReadObjectDataAsync().Result;
            
            foreach (var result in list)
            {
                Console.WriteLine($"Id:{result.Id}, X:{result.X}, Y:{result.Y}");
            }
        }
    }
}    