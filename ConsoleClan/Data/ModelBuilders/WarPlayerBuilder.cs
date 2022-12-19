using ConsoleClan.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace ConsoleClan.Data.ModelBuilders
{
	internal static class WarPlayerBuilder
	{
		public static ModelBuilder Configure<T>(this ModelBuilder modelBuilder)
			where T : WarPlayer
		{
			return modelBuilder.Entity<T>(entity =>
			{
				entity.ToTable($"{nameof(WarPlayer)}");
				entity.HasKey(entity => new { entity.WarId, entity.PlayerId });
			});
		}
	}
}
