using ConsoleClan.Data.Entities;

namespace ConsoleClan.Data.Interfaces
{
	public interface IPlayerRepository
	{
		Task InsertAsync(Player player);
		Task<IEnumerable<Player>> SelectAsync(IEnumerable<string> tags);
		Task<IEnumerable<Player>> SelectForActivityAsync(bool hasLeft);
		Task<IEnumerable<Player>> SelectNotInAsync(IEnumerable<string> notInTags, bool hasLeft);
		Task UpdateAsync(IEnumerable<Player> players);
	}
}
