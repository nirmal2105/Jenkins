using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AwsS3FileApiAssignment.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("get-all-buckets")]
        public async Task<IActionResult> GetAllBucketAsync()
        {
            //var data = await _s3Client.ListBucketsAsync();
            //var buckets = data.Buckets.Select(b => { return b.BucketName; });
            //return Ok(buckets);
            var s3Client = new AmazonS3Client("AKIA4D4UH226PM35K74M", "wKfRbCo7udYsN8NOP19r2Zs8N2OgY3aWDOvABS9K", Amazon.RegionEndpoint.USEast1);
            var buckets = await s3Client.ListBucketsAsync();
            //Console.WriteLine(String.Join(",", buckets.Buckets.Select(b => b.BucketName)));
            return Ok(String.Join(",", buckets.Buckets.Select(b => b.BucketName)));

        }

        [HttpGet, Route("ReadBucket")]
        public async Task<IActionResult> GetAll()
        {

            string UID = "AKIA4D4UH226PM35K74M ";
            string secret = "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";
            var s3client = new AmazonS3Client(UID, secret, Amazon.RegionEndpoint.USEast1);
            var buckets = await s3client.ListBucketsAsync();
            foreach (var bucket in buckets.Buckets)
            {
                var objects = await s3client.ListObjectsAsync(bucket.BucketName);

                if (objects != null)
                {
                    //Console.WriteLine($"for bucket {bucket.BucketName},files:{string.Join(",", objects.S3Objects.Select(x => x.Key))}");
                    foreach (var s3object in objects.S3Objects)
                    {
                        var objectResponse = await s3client.GetObjectAsync(new GetObjectRequest { BucketName = bucket.BucketName, Key = s3object.Key, });
                        var bytes = new byte[objectResponse.ResponseStream.Length]; 
                        objectResponse.ResponseStream.Read(bytes, 0, bytes.Length); 
                        return StatusCode(200, Encoding.UTF8.GetString(bytes));
                    }
                }

            }
            return BadRequest();





        }




    }
}
