using ConsoleClan.Models;

namespace ConsoleClan
{
	internal class MemberScore
	{
		public MemberDetail MemberDetail { get; set; }
		public List<Battle> Battles { get; private set; } = new List<Battle>();
		public int DonationPenalty { get; private set; }

		public MemberScore(MemberDetail memberDetail)
		{
			MemberDetail = memberDetail;
			CalculateDonationPenalty();
		}

		private void CalculateDonationPenalty()
		{
			var donations = MemberDetail.donations;
			var donationsReceived = MemberDetail.donationsReceived;
			var difference = donations - donationsReceived;
			if (difference < 0)
			{
				DonationPenalty = Math.Abs(difference / 250);
			}
			else
			{
				DonationPenalty = 0;
			}
		}

		public int CalculateTotalScore()
		{
			var penalties = DonationPenalty + Battles.Sum(battle => battle.Penalty);
			var points = Battles.Sum(battle => battle.Stars);
			return points - penalties;
		}

		public int CalculateTotalScoreForMigration()
		{
			var points = Battles.Sum(battle => battle.Stars);
			return points - DonationPenalty;
		}
	}
}
