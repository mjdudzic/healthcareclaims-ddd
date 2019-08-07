using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Integration.Tests
{
	public class BatchesControllerTests : ScenarioBase
	{
		[Fact]
		public async Task GivenBatchFileToSubmit_WhenSubmitted_ApiReturnsSuccessStatusCode()
		{
			using (var server = CreateServer())
			{
				// Arrange
				var client = server.CreateClient();

				var file = await File.ReadAllBytesAsync("batch1.json");

				var content = new MultipartFormDataContent
				{
					{new StringContent(Guid.NewGuid().ToString()), "HealthcareProviderId"}
				};

				var byteArrayContent = new ByteArrayContent(file);
				byteArrayContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
				byteArrayContent.Headers.ContentDisposition.Name = "\"BatchJsonFile\"";
				byteArrayContent.Headers.ContentDisposition.FileName = "/c:/batches1.json";
				content.Add(byteArrayContent);

				// Act
				var response = await client.PostAsync("api/batches", content);

				// Assert
				response.IsSuccessStatusCode.Should().BeTrue();
			}

		}
	}
}
