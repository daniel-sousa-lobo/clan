using System.Reflection.Emit;

namespace ConsoleClan.Models
{
	public record ClanDetail
	{
		public string? tag { get; set; }
		public string? name { get; set; }
		public string? type { get; set; }
		public string? description { get; set; }
		public Location? location { get; set; }
		public BadgeUrls? badgeUrls { get; set; }
		public int? clanLevel { get; set; }
		public int? clanPoints { get; set; }
		public int? clanVersusPoints { get; set; }
		public int? requiredTrophies { get; set; }
		public string? warFrequency { get; set; }
		public int? warWinStreak { get; set; }
		public int? warWins { get; set; }
		public int? warTies { get; set; }
		public int? warLosses { get; set; }
		public bool? isWarLogPublic { get; set; }
		public WarLeague? warLeague { get; set; }
		public int? members { get; set; }
		public IEnumerable<MemberDetail>? memberList { get; set; }
		public IEnumerable<Label>? labels { get; set; }
		public int? requiredVersusTrophies { get; set; }
		public int? requiredTownhallLevel { get; set; }
		public ChatLanguage? chatLanguage { get; set; }
	}
}
