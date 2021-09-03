using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;
using Usabit.Framework.AWS.Abstractions;
using Usabit.Framework.AWS.Options;

namespace Usabit.Framework.AWS.Concrete
{
    public sealed class S3Service : IS3Service
    {
        private S3Options _s3Options;
        private BasicAWSCredentials _credentials => new BasicAWSCredentials(_s3Options.AccessKey, _s3Options.SecretKey);

        public S3Service(IOptions<S3Options> options)
        {
            _s3Options = options.Value;
        }

        public async Task<GetObjectResponse> DownloadAsync(string key, RegionEndpoint regionEndpoint = null)
        {
            GetObjectResponse response = null;

            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = _s3Options.BucketName,
                Key = key
            };

            using (AmazonS3Client client = GetS3Client(regionEndpoint))
            {
                response = await client.GetObjectAsync(request);
            }

            return response;
        }

        public async Task<Stream> DownloadStreamAsync(string key, RegionEndpoint regionEndpoint = null)
        {
            MemoryStream response = new MemoryStream();
            GetObjectResponse objectResponse = await DownloadAsync(key, regionEndpoint);

            using (Stream awsStream = objectResponse.ResponseStream)
            {
                awsStream.CopyTo(response);
                response.Seek(0, SeekOrigin.Begin);
            }

            return response;
        }

        public async Task<PutObjectResponse> UploadAsync(Stream stream, string key, RegionEndpoint regionEndpoint = null)
        {
            PutObjectResponse response = null;

            using (AmazonS3Client client = GetS3Client(regionEndpoint))
            {
                PutObjectRequest request = new PutObjectRequest
                {
                    BucketName = _s3Options.BucketName,
                    Key = key,
                    InputStream = stream
                };

                response = await client.PutObjectAsync(request);
            }

            return response;
        }

        private AmazonS3Client GetS3Client(RegionEndpoint regionEndpoint = null)
        {
            if (regionEndpoint == null)
                regionEndpoint = RegionEndpoint.USEast1;

            return new AmazonS3Client(_credentials, regionEndpoint);
        }
    }
}
