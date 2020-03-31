using System;
using System.Collections.Generic;
using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Amazon;
using Amazon.Internal;
using Newtonsoft.Json;


namespace GasMonitor
{
    public static class AmazonService
    {
        private static readonly string BucketName = Environment.GetEnvironmentVariable("S3Bucket");
        private static readonly string KeyName = "locations.json";
        private static RegionEndpoint BucketRegion = RegionEndpoint.EUWest2;
        private static readonly IAmazonS3 Client = new AmazonS3Client(Environment.GetEnvironmentVariable("S3AccessKey"), Environment.GetEnvironmentVariable("S3SecretKey"), BucketRegion);
        
        public static async Task<List<Location>> ReadObjectDataAsync()
        {
            string responseBody = "";
            List<Location> locationObjects = new List<Location>();
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = BucketName,
                    Key = KeyName
                };
                using (GetObjectResponse response = await Client.GetObjectAsync(request))
                using (Stream responseStream = response.ResponseStream)
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    responseBody = reader.ReadToEnd();
                    locationObjects = JsonConvert.DeserializeObject<List<Location>>(responseBody);
                }
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }

            return locationObjects;
        }
    }
}