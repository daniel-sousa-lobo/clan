namespace ConsoleClan.Data.Entities
{
	public record Player
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public string Tag { get; set; } = null!;
		public int Donations { get; set; }
		public int DonationsReceived { get; set; }
		public bool HasLeft { get; set; }
	}
}
