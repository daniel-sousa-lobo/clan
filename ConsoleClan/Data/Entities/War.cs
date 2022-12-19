namespace ConsoleClan.Data.Entities
{
	public record War
	{
		public int Id { get; set; }
		public string? Tag { get; set; } = null!;
		public DateTimeOffset? EndTime { get; set; }
		public DateTimeOffset? PreparationStartTime { get; set; }
		public DateTimeOffset StartTime { get; set; }
		public string? State { get; set; }
		public int TeamSize { get; set; }
	}
}
