using ConsoleClan.Data.Entities;

namespace ConsoleClan.Data.Interfaces
{
	public interface ILeagueWarRepository
	{
		Task InsertAsync(LeagueWar leagueWar);
		Task<LeagueWar?> SelectAsync(int leagueId, int warId);
		Task<IEnumerable<LeagueWar>> SelectAsync(IEnumerable<int> warIds);
	}
}
