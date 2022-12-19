namespace ConsoleClan.Data.Entities
{
	public record Attack
	{
		public int Id { get; set; }
		public int PlayerId { get; set; }
		public int WarId { get; set; }
		public string DefenderTag { get; set; } = null!;
		public int DestructionPercentage { get; set; }
		public int Duration { get; set; }
		public int Order { get; set; }
		public int Stars { get; set; }
	}
}
