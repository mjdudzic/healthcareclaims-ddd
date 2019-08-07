using Infrastructure.Common.Web.Minio.Interfaces;

namespace Infrastructure.Common.Web.Minio.Configuration
{
	public class ObjectsStorageServiceConfiguration : IObjectsStorageServiceConfiguration
	{
		public string Endpoint { get; set; }
		public string AccessKey { get; set; }
		public string SecretKey { get; set; }
	}
}