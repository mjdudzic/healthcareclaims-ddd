using System.IO;
using System.Threading.Tasks;
using Infrastructure.Common.Web.Minio.Interfaces;
using Infrastructure.Common.Web.Minio.Model;
using Minio.Exceptions;

namespace Infrastructure.Common.Web.Minio
{
	public class ObjectsStorageService : IObjectsStorageService
	{
		private readonly IMinioClientFactory _minioClientFactory;

		public ObjectsStorageService(IMinioClientFactory minioClientFactory)
		{
			_minioClientFactory = minioClientFactory;
		}

		public async Task UploadObjectAsync(
			string bucketName,
			string objectName,
			Stream objectStream)
		{
			var client = _minioClientFactory.GetClient();

			if (await client.BucketExistsAsync(bucketName) == false)
			{
				await client.MakeBucketAsync(bucketName);
			}

			await client.PutObjectAsync(
				bucketName,
				objectName,
				objectStream,
				objectStream.Length);
		}

		public async Task<ObjectResult> GetObjectContentAsync(
			string bucketName,
			string objectName)
		{
			var client = _minioClientFactory.GetClient();

			try
			{
				await client.StatObjectAsync(bucketName, objectName);

				var fileContent = string.Empty;
				await client.GetObjectAsync(
					bucketName,
					objectName,
					async stream =>
					{
						using (var streamReader = new StreamReader(stream))
						{
							fileContent = await streamReader.ReadToEndAsync();
						}
					});

				return new ObjectResult
				{
					BucketName = bucketName,
					ObjectName = objectName,
					Content = fileContent
				};
			}
			catch (MinioException)
			{
				// Object does not exist
				return null;
			}
		}

		public Task<string> GetObjectPresignedUrlAsync(
			string bucketName,
			string objectName,
			int expirationThresholdInSecs)
		{
			var client = _minioClientFactory.GetClient();

			return client.PresignedGetObjectAsync(
				bucketName,
				objectName,
				expirationThresholdInSecs);
		}
	}
}
