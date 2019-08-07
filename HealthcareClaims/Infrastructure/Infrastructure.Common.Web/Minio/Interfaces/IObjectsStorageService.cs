using System.IO;
using System.Threading.Tasks;
using Infrastructure.Common.Web.Minio.Model;

namespace Infrastructure.Common.Web.Minio.Interfaces
{
	public interface IObjectsStorageService
	{
		Task<ObjectResult> GetObjectContentAsync(string bucketName, string objectName);
		Task<string> GetObjectPresignedUrlAsync(string bucketName, string objectName, int expirationThresholdInSecs);
		Task UploadObjectAsync(string bucketName, string objectName, Stream objectStream);
	}
}