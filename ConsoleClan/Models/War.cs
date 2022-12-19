namespace ConsoleClan.Models
{
	public record War
	{
		public string? state { get; set; }
		public int teamSize { get; set; }
		public DateTimeOffset? preparationStartTime { get; set; }
		public DateTimeOffset startTime { get; set; }
		public DateTimeOffset? endTime { get; set; }
		public Clan? clan { get; set; }
		public Clan? opponent { get; set; }
	}
}
