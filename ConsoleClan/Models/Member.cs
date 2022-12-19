namespace ConsoleClan.Models
{
	public record Member
	{
		public string tag { get; set; } = null!;
		public string? name { get; set; }
		public int? townhallLevel { get; set; }
		public int? mapPosition { get; set; }
		public IEnumerable<Attack>? attacks { get; set; }
	}
}
