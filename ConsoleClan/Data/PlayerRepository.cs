using ConsoleClan.Data.Entities;
using ConsoleClan.Data.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System.Diagnostics;

namespace ConsoleClan.Data
{
	internal class PlayerRepository : IPlayerRepository
	{
		private readonly IDbContextFactory<Context> contextFactory;
		private readonly ILogger<IPlayerRepository> logger;

		public PlayerRepository(IDbContextFactory<Context> contextFactory, ILogger<IPlayerRepository> logger)
		{
			this.contextFactory = contextFactory;
			this.logger = logger;
		}

		public async Task InsertAsync(Player player)
		{
			try
			{
				using (var context = contextFactory.CreateDbContext())
				{
					context.PlayerSet.Add(player);
					await context.SaveChangesAsync();
				}
			}
			catch (Exception exception)
			{
				throw ExceptionHandler.Create(
					logger,
					exception,
					($"{nameof(player)}", player));
			}
		}

		public async Task<IEnumerable<Player>> SelectAsync(IEnumerable<string> tags)
		{
			try
			{
				using (var context = contextFactory.CreateDbContext())
				{
					return await (
						from player in context.PlayerSet
						where tags.Contains(player.Tag)
						select player)
						.ToListAsync();
				}
			}
			catch (Exception exception)
			{
				throw ExceptionHandler.Create(
					logger,
					exception,
					($"{nameof(tags)}", tags));
			}
		}

		public async Task<IEnumerable<Player>> SelectNotInAsync(IEnumerable<string> notIntags, bool hasLeft)
		{
			try
			{
				using (var context = contextFactory.CreateDbContext())
				{
					return await (
						from player in context.PlayerSet
						where
							!notIntags.Contains(player.Tag) &&
							player.HasLeft == hasLeft
						select player)
						.ToListAsync();
				}
			}
			catch (Exception exception)
			{
				throw ExceptionHandler.Create(
					logger,
					exception,
					($"{nameof(notIntags)}", notIntags),
					($"{nameof(hasLeft)}", hasLeft));
			}
		}

		public async Task<IEnumerable<Player>> SelectForActivityAsync(bool hasLeft)
		{
			try
			{
				using (var context = contextFactory.CreateDbContext())
				{
					return await (
						from player in context.PlayerSet
						where
							player.HasLeft == hasLeft
						select player)
						.ToListAsync();
				}
			}
			catch (Exception exception)
			{
				throw ExceptionHandler.Create(
					logger,
					exception,
					($"{nameof(hasLeft)}", hasLeft));
			}
		}

		public async Task UpdateAsync(IEnumerable<Player> players)
		{
			try
			{
				using (var context = contextFactory.CreateDbContext())
				{
					context.PlayerSet.UpdateRange(players);
					await context.SaveChangesAsync();
				}
			}
			catch (Exception exception)
			{
				throw ExceptionHandler.Create(
					logger,
					exception,
					($"{nameof(players)}", players));
			}
		}

	}
}
