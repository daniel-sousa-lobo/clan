namespace ConsoleClan.Models
{
	public record Clan
	{
		public string? tag { get; set; }
		public string? name { get; set; }
		public BadgeUrls? badgeUrls { get; set; }
		public int? clanLevel { get; set; }
		public int? attacks { get; set; }
		public int? stars { get; set; }
		public float? destructionPercentage { get; set; }
		public IEnumerable<Member>? members { get; set; }
	}
}
