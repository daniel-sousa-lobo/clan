using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsoleClan
{
	public class Startup
	{
		public IServiceProvider Provider { get; private set; }
		private readonly IHost application;
		private readonly IHostBuilder hostBuilder;

		public Startup()
		{
			var basePath = Directory.GetParent(AppContext.BaseDirectory)?.FullName;
			if (basePath == null)
			{
				throw new NullReferenceException("Null base path");
			}

			hostBuilder = Host.CreateDefaultBuilder()
			.ConfigureAppConfiguration(application => ConfigureApplication(application))
			.ConfigureServices((context, services) => ConfigureServices(context, services));
			application = hostBuilder.Build();
			Provider = application.Services;
		}

		private void ConfigureApplication(IConfigurationBuilder configurationBuilder)
		{
			configurationBuilder.AddJsonFile("appsettings.json");
		}

		private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
		{
			var configuration = context.Configuration;

			services.AddSingleton<ILoggerFactory, LoggerFactory>();
			services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

			services.AddLogging(builder =>
			{
				builder.AddConfiguration(configuration.GetSection("Logging"));
				builder.AddConsole();
				builder.AddDebug();
				builder.AddEventLog();
			});

			//Services
			services.AddSingleton(configuration);

			int commandTimeoutSeconds = configuration.GetValue<int>("DbCommandTimeoutSeconds");
			string connectionString = configuration.GetConnectionString("Connection");

			ServiceConfiguration.Configure(services, configuration, commandTimeoutSeconds, connectionString);
			Data.ServiceConfiguration.Configure(services, configuration, commandTimeoutSeconds, connectionString);
		}

		public void Run()
		{
			if (application == null)
			{
				throw new NullReferenceException($"{nameof(application)} is null");
			}
			application.Run();
		}
	}
}