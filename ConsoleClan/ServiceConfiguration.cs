using ConsoleClan.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleClan
{
	public class ServiceConfiguration
	{
		public static void Configure(IServiceCollection services, IConfiguration configuration, int commandTimeoutSeconds, string xpandedRawConnectionString)
		{
			services.AddTransient<IClash, Clash>();
		}
	}
}
