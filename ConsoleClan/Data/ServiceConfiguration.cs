using ConsoleClan.Data.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleClan.Data
{

	public class ServiceConfiguration
	{
		public static void Configure(IServiceCollection services, IConfiguration configuration, int commandTimeoutSeconds, string connectionString)
		{
			services.AddDbContextFactory<Context>(options =>
				{
					options.UseSqlServer(
						connectionString,
						providerOptions => providerOptions.CommandTimeout(commandTimeoutSeconds));
					options.EnableSensitiveDataLogging();
				},
				ServiceLifetime.Transient);

			services.AddTransient<IPlayerRepository, PlayerRepository>();
			services.AddTransient<ILeagueRepository, LeagueRepository>();
			services.AddTransient<IWarRepository, WarRepository>();
			services.AddTransient<ILeagueWarRepository, LeagueWarRepository>();
			services.AddTransient<IAttackRepository, AttackRepository>();
			services.AddTransient<IPlayerRepository, PlayerRepository>();
			services.AddTransient<IWarPlayerRepository, WarPlayerRepository>();
		}
	}
}