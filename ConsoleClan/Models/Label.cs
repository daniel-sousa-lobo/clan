namespace ConsoleClan.Models
{
	public record Label
	{
		public int? id { get; set; }
		public string? name { get; set; }
		public IconUrls? iconUrls { get; set; }
	}
}
