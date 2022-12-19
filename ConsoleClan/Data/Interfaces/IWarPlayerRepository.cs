using ConsoleClan.Data.Entities;

namespace ConsoleClan.Data.Interfaces
{
	public interface IWarPlayerRepository
	{
		Task InsertAsync(WarPlayer warPlayer);
		Task<IEnumerable<WarPlayer>> SelectAsync(int playerId, IEnumerable<int> warIds);
	}
}