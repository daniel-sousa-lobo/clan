using ConsoleClan.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace ConsoleClan.Data.ModelBuilders
{
	internal static class LeagueWarBuilder
	{
		public static ModelBuilder Configure<T>(this ModelBuilder modelBuilder)
			where T : LeagueWar
		{
			return modelBuilder.Entity<T>(entity =>
			{
				entity.ToTable($"{nameof(LeagueWar)}");
				entity.HasKey(entity => new { entity.LeagueId, entity.WarId });
			});
		}
	}
}
