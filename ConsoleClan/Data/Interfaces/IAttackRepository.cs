using ConsoleClan.Data.Entities;

namespace ConsoleClan.Data.Interfaces
{
	public interface IAttackRepository
	{
		Task InsertAsync(Attack attack);
		Task UpdateAsync(Attack attack);
		Task<Attack?> SelectAsync(int playerId, int warId, int order);
		Task<IEnumerable<Attack>> SelectAsync(int id, IEnumerable<int> warIds);
	}
}
