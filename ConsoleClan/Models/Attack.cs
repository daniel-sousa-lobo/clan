namespace ConsoleClan.Models
{
	public record Attack
	{
		public string attackerTag { get; set; } = null!;
		public string defenderTag { get; set; } = null!;
		public int stars { get; set; }
		public int destructionPercentage { get; set; }
		public int order { get; set; }
		public int duration { get; set; }
	}
}
