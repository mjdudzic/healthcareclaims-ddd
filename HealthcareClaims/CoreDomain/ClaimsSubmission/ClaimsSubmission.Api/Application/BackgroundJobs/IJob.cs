using System.Threading.Tasks;
using Hangfire;
using Hangfire.Server;

namespace ClaimsSubmission.Api.Application.BackgroundJobs
{
	public interface IJob<in T>
	{
		Task Execute(T item, PerformContext context, IJobCancellationToken cancellationToken);
	}
}