using Microsoft.EntityFrameworkCore;

namespace InsuredMembers.Api.Infrastructure.Persistence
{
	public class MembersContext : DbContext
	{
		public MembersContext(DbContextOptions options)
			: base(options)
		{
		}

		public DbSet<Member> Members { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder
				.Entity<Member>(entity =>
				{
					entity.HasKey(i => i.Id);
					entity.Property(i => i.Id).ValueGeneratedNever();
				});
		}
	}
}