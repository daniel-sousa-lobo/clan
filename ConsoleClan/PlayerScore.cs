namespace ConsoleClan
{
	internal class PlayerScore
	{
		private int totalScore = 0;
		private int totalStars = 0;
		private int totalPenalty = 0;
		public string Name { get; set; } = null!;
		public List<PlayerAttack> PlayerAttacks { get; set; } = new();
		public int TotalScore { get { GetTotalScore(); return totalScore; } }
		public int Donations { get; set; } = 0;
		public int DonationsReceived { get; set; } = 0;
		public int TotalStars { get { GetTotalScore(); return totalStars; } }
		public int TotalPenalty { get { GetTotalScore(); return totalPenalty; } }

		public void GetTotalScore()
		{
			totalScore = 0;
			totalStars = 0;
			totalPenalty = 0;
			foreach (var playerAttack in PlayerAttacks)
			{
				foreach (var battle in playerAttack.Battles)
				{
					if (battle == null)
					{
						totalPenalty += 1;
						continue;
					}
					totalPenalty += battle.Penalty;
					totalStars += battle.Stars;
				}
			}
			if (Donations - DonationsReceived < 0)
			{
				totalPenalty = (DonationsReceived - Donations) / 250;
			}
			totalScore = totalStars - totalPenalty;
		}
	}
}
