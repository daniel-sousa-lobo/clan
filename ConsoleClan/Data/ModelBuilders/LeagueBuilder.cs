using ConsoleClan.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace ConsoleClan.Data.ModelBuilders
{
	internal static class LeagueBuilder
	{
		public static ModelBuilder Configure<T>(this ModelBuilder modelBuilder)
			where T : League
		{
			return modelBuilder.Entity<T>(entity =>
			{
				entity.ToTable($"{nameof(League)}");
			});
		}
	}
}
