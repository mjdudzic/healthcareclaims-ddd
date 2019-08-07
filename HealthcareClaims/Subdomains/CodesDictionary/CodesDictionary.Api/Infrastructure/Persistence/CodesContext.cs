using Microsoft.EntityFrameworkCore;

namespace CodesDictionary.Api.Infrastructure.Persistence
{
	public class CodesContext : DbContext
	{
		public CodesContext(DbContextOptions options)
			: base(options)
		{
		}

		public DbSet<TreatmentCode> TreatmentCodes { get; set; }
		public DbSet<MedicineCode> MedicineCodes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder
				.Entity<TreatmentCode>(entity =>
				{
					entity.HasKey(i => i.Id);
					entity.Property(i => i.Id).ValueGeneratedNever();
				});

			modelBuilder
				.Entity<MedicineCode>(entity =>
				{
					entity.HasKey(i => i.Id);
					entity.Property(i => i.Id).ValueGeneratedNever();
				});
		}
	}
}