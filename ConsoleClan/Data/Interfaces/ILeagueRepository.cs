using ConsoleClan.Data.Entities;

namespace ConsoleClan.Data.Interfaces
{
	public interface ILeagueRepository
	{
		Task InsertAsync(League league);
		Task<League?> SelectAsync(string season);
	}
}
