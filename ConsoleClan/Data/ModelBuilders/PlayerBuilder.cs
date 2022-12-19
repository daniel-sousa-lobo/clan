using ConsoleClan.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace ConsoleClan.Data.ModelBuilders
{
	internal static class PlayerBuilder
	{
		public static ModelBuilder Configure<T>(this ModelBuilder modelBuilder)
			where T : Player
		{
			return modelBuilder.Entity<T>(entity =>
			{
				entity.ToTable($"{nameof(Player)}");
			});
		}
	}
}
