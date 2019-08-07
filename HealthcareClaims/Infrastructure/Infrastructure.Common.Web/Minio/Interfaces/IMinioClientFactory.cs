using Minio;

namespace Infrastructure.Common.Web.Minio.Interfaces
{
	public interface IMinioClientFactory
	{
		MinioClient GetClient();
	}
}