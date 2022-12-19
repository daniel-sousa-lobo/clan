namespace ConsoleClan.Models
{
	public record ChatLanguage
	{
		public int? id { get; set; }
		public string? name { get; set; }
		public string? languageCode { get; set; }
	}
}