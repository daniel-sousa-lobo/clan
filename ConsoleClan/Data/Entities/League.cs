namespace ConsoleClan.Data.Entities
{
	public record League
	{
		public int Id { get; set; }
		public string Season { get; set; } = null!;
	}
}
