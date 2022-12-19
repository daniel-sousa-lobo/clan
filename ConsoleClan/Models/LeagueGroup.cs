namespace ConsoleClan.Models
{
	public record LeagueGroup
	{
		public string? state { get; set; }
		public string season { get; set; } = null!;
		public IEnumerable<Clan>? clans { get; set; }
		public IEnumerable<Round>? rounds { get; set; }
	}
}
