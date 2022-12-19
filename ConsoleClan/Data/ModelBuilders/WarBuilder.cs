using ConsoleClan.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace ConsoleClan.Data.ModelBuilders
{
	internal static class WarBuilder
	{
		public static ModelBuilder Configure<T>(this ModelBuilder modelBuilder)
			where T : War
		{
			return modelBuilder.Entity<T>(entity =>
			{
				entity.ToTable($"{nameof(War)}");
			});
		}
	}
}
