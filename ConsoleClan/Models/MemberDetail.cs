using ConsoleClan.Data.Entities;

namespace ConsoleClan.Models
{
	public record MemberDetail
	{
		public string tag { get; set; } = null!;
		public string name { get; set; } = null!;
		public string? role { get; set; }
		public int? expLevel { get; set; }
		public League? league { get; set; }
		public int? trophies { get; set; }
		public int? versusTrophies { get; set; }
		public int? clanRank { get; set; }
		public int? previousClanRank { get; set; }
		public int donations { get; set; }
		public int donationsReceived { get; set; }
	}
}