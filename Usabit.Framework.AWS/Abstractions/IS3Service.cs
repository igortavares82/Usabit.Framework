using Amazon;
using Amazon.S3.Model;
using System.IO;
using System.Threading.Tasks;

namespace Usabit.Framework.AWS.Abstractions
{
    public interface IS3Service
    {
        Task<Stream> DownloadStreamAsync(string key, RegionEndpoint regionEndpoint = null);
        Task<GetObjectResponse> DownloadAsync(string key, RegionEndpoint regionEndpoint = null);
        Task<PutObjectResponse> UploadAsync(Stream stream, string key, RegionEndpoint regionEndpoint = null);
    }
}
