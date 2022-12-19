using ConsoleClan.Data.Entities;

namespace ConsoleClan.Data.Interfaces
{
	public interface IWarRepository
	{
		Task InsertAsync(War war);
		Task<War?> SelectAsync(string tag);
		Task<War?> SelectAsync(string? tag, DateTimeOffset preparationStartTime);
		Task<IEnumerable<War>> SelectAsync(DateTimeOffset startDate, DateTimeOffset endDate);
		Task UpdateAsync(War war);
	}
}
