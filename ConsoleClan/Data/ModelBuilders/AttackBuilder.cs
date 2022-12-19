using ConsoleClan.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace ConsoleClan.Data.ModelBuilders
{
	internal static class AttackBuilder
	{
		public static ModelBuilder Configure<T>(this ModelBuilder modelBuilder)
			where T : Attack
		{
			return modelBuilder.Entity<T>(entity =>
			{
				entity.ToTable($"{nameof(Attack)}");
			});
		}
	}
}
