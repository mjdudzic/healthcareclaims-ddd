using System.Threading.Tasks;
using Hangfire;
using Hangfire.Server;

namespace ClaimsVetting.Api.Application.BackgroundJobs
{
	public interface IJob<in T>
	{
		Task Execute(T item, PerformContext context, IJobCancellationToken cancellationToken);
	}
}