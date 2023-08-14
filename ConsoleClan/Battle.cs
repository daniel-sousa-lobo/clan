namespace ConsoleClan
{
	internal class Battle
	{
		public static readonly int MaximumPositionDifference = 10;
		public int Ordinal { get; }
		public DateTimeOffset StartTime { get; }
		public int Stars { get; set; }
		public int MapPosition { get; set; }
		public int EnemyMapPosition { get; set; }
		public BattleTypeReference BattleType { get; set; }
		public int Penalty { get; private set; }

		public Battle(int ordinal, DateTimeOffset startTime, int stars, int mapPosition, int enemyMapPosition, BattleTypeReference battleType)
		{
			Ordinal = ordinal;
			StartTime = startTime;
			Stars = stars;
			MapPosition = mapPosition;
			EnemyMapPosition = enemyMapPosition;
			BattleType = battleType;
			CalculateBattlePenalty();
		}

		private void CalculateBattlePenalty()
		{
			Penalty = 0;
			var positionDifference = Math.Abs(EnemyMapPosition - MapPosition);
			if (positionDifference > MaximumPositionDifference)
			{
				Penalty = positionDifference / MaximumPositionDifference;
			}
			else
			{
				Penalty = 0;
			}
		}
	}
}
