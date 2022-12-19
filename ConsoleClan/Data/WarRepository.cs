using ConsoleClan.Data.Entities;
using ConsoleClan.Data.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ConsoleClan.Data
{
	internal class WarRepository : IWarRepository
	{
		private readonly IDbContextFactory<Context> contextFactory;
		private readonly ILogger<IWarRepository> logger;

		public WarRepository(IDbContextFactory<Context> contextFactory, ILogger<IWarRepository> logger)
		{
			this.contextFactory = contextFactory;
			this.logger = logger;
		}

		public async Task InsertAsync(War war)
		{
			try
			{
				using (var context = contextFactory.CreateDbContext())
				{
					context.WarSet.Add(war);
					await context.SaveChangesAsync();
				}
			}
			catch (Exception exception)
			{
				throw ExceptionHandler.Create(
					logger,
					exception,
					($"{nameof(war)}", war));
			}
		}

		public async Task<IEnumerable<War>> SelectAsync(DateTimeOffset startDate, DateTimeOffset endDate)
		{
			try
			{
				using (var context = contextFactory.CreateDbContext())
				{
					var queriable = (
						from war in context.WarSet
						where
							war.StartTime >= startDate &&
							war.StartTime < endDate
						select war);
					return await queriable.ToListAsync();
				}
			}
			catch (Exception exception)
			{
				throw ExceptionHandler.Create(
					logger,
					exception,
					($"{nameof(startDate)}", startDate),
					($"{nameof(endDate)}", endDate));
			}
		}

		public Task<War?> SelectAsync(string tag)
		{
			return SelectAsyncPrivate(tag, null);
		}

		public Task<War?> SelectAsync(string? tag, DateTimeOffset preparationStartTime)
		{
			return SelectAsyncPrivate(tag, preparationStartTime);
		}

		private async Task<War?> SelectAsyncPrivate(string? tag, DateTimeOffset? preparationStartTime)
		{
			try
			{
				using (var context = contextFactory.CreateDbContext())
				{
					var queriable = (
						from war in context.WarSet
						where
							(tag == null || war.Tag == tag) &&
							(preparationStartTime == null || war.PreparationStartTime == preparationStartTime)
						select war);
					return await queriable.SingleOrDefaultAsync();
				}
			}
			catch (Exception exception)
			{
				throw ExceptionHandler.Create(
					logger,
					exception,
					($"{nameof(tag)}", tag),
					($"{nameof(preparationStartTime)}", preparationStartTime));
			}
		}

		public async Task UpdateAsync(War war)
		{
			try
			{
				using (var context = contextFactory.CreateDbContext())
				{
					context.WarSet.Update(war);
					await context.SaveChangesAsync();
				}
			}
			catch (Exception exception)
			{
				throw ExceptionHandler.Create(
					logger,
					exception,
					($"{nameof(war)}", war));
			}
		}
	}
}
