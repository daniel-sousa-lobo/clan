namespace ConsoleClan
{
	internal class PlayerScore
	{
		public string Name { get; set; } = null!;
		public List<PlayerAttack> PlayerAttacks { get; set; } = new();
		public int Score { get; set; } = 0;
		public int Donations { get; set; } = 0;
		public int DonationsReceived { get; set; } = 0;
		public int Stars { get; set; } = 0;
		public int Penalty { get; set; } = 0;
	}
}
